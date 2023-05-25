using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using back.Classes;
using back.Classes.SessionState;
using back.DAL;
using back.Entities;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;

namespace back.Services;

public interface ISessionService
{
    public Task<SessionDTO> getSession(string sessionIdentifier);

    public Task<bool> addUserToSession(int id, string sessionIdentifier);

    public Task<SessionDTO> createSession(IFormFile jiraFile);

    // public Task<NoteEntity> createNote(int userID, int cardNumber);

    public Task<bool> voteForCurrentUS(int userID, int cardNumber, string sessionIdentifier);
    
    public Task<bool> userReadySession(int userID, string sessionIdentifier);
    
    public Task<bool> userNotReadySession(int userID, string sessionIdentifier);


    public Task<UserStoryPropositionEntity> createUserStoryProposition(UserStoryPropositionInput usInput, string sessionIdentifier);

    public Task addWS(WebSocket webSocket, string sessionIdentifier);

    public void removeWS(WebSocket webSocket, string sessionIdentifier);

    public Task sendSessionToAllWS(string sessionIdentifier);

    public Task sendUSToAllWS(string sessionIdentifier);

    public Task showVotesOfEveryone(string sessionIdentifier);

    public CustomFile downloadCSV(string sessionIdentifier);

}

public class CustomFile
{
    public byte[] FileContents { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}

public class SessionService : ISessionService
{
    private readonly IUserStoryPropositionService _userStoryPropositionService;

    private readonly IUserStoryService _userStoryService;

    private readonly DatabaseContext _databaseContext;

    private readonly IUserService _userService;
    
    //private Session _currentSession { get; set; }

    public SessionService(IUserStoryPropositionService userStoryPropositionService, IUserStoryService userStoryService,DatabaseContext databaseContext, IUserService userService)
    {
        _userStoryPropositionService = userStoryPropositionService;
        _userStoryService = userStoryService;
        _databaseContext = databaseContext;
        _userService = userService;
    }

    public async Task<SessionDTO> getSession(string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));
        
        if (session == null)
        {
            return null;
        }

        if (session._joinedUsers == null || session._allUserStories == null)
        {
            return null;
        }
        
        var sessionDTO = new SessionDTO();
        sessionDTO.currentUserStory = session.currentUserStoryDiscussed();
        var users = session._joinedUsers;
        sessionDTO.users = new List<UserDTO>(users.Select(user => new UserDTO { id = user.id, name = user.name}));
        sessionDTO.state = session._state.ToString();
        sessionDTO.usersNotes = session._state.getUsersVote();
        sessionDTO.nb_ws = session._WebSockets.Count;
        return sessionDTO;
    }

    public async Task<bool> addUserToSession(int id, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return false;
        }
        
        // if user does not exist
        if (await _userService.GetByID(id) == null)
        {
            return false;
        }
        
        // if user already in session
        if (session._joinedUsers.Select(user => user.id).Contains(id))
        {
            return false;
        }

        UserDTO user = await _userService.GetByID(id);
        session.addUser(new UserEntity { id = user.id, name = user.name});
        
        return true;
    }

    public async Task<SessionDTO> createSession(IFormFile? jiraFile) //TO DO: handle jira file
    {
        Session newSession = new Session();
        string identifier = GenerateUniqueId();
        newSession.Identifier = identifier;
        
        // keep track of every sessions
        SessionList.Add(newSession);

        // parsing of jira file
        if (jiraFile != null && jiraFile.FileName.Substring(jiraFile.FileName.Length-4).Equals(".csv"))
        {
            await parseJiraFile(newSession, jiraFile);
            await sendUSToAllWS(newSession.Identifier);
        }

        SessionDTO result = new SessionDTO();
        var users = newSession._joinedUsers;
        result.users = new List<UserDTO>(users.Select(user => new UserDTO { id = user.id, name = user.name }));
        result.currentUserStory = newSession.currentUserStoryDiscussed();
        result.state = newSession._state.ToString();
        result.usersNotes = null;
        result.identifier = newSession.Identifier;

        return result; 
    }

    private async Task parseJiraFile(Session session, IFormFile jiraFile)
    {
        using (var reader = new StreamReader(jiraFile.OpenReadStream()))
        {
            using (var csvParser = new TextFieldParser(reader))
            {
                csvParser.SetDelimiters(",");
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Read the header row
                var headerFields = csvParser.ReadFields();

                // Find the index of the summary column
                int summaryColumnIndex = Array.IndexOf(headerFields, "Summary");

                List<string> summaryValues = new List<string>();

                while (!csvParser.EndOfData)
                {
                    var fields = csvParser.ReadFields();
                        
                    // Check if the row has enough columns
                    if (fields.Length > summaryColumnIndex)
                    {
                        summaryValues.Add(fields[summaryColumnIndex]);
                    }
                }

                // add users stories to the session
                Stack<UserStoryPropositionEntity> uspList = new Stack<UserStoryPropositionEntity>();

                foreach (string summary in summaryValues)
                {
                    UserStoryPropositionInput newusp = new UserStoryPropositionInput();
                    newusp.description = summary;
                                        
                    UserStoryPropositionEntity usp = await _userStoryPropositionService.create(newusp);
                                        
                    uspList.Push(usp);
                }
                
                session._allUserStories = new Stack<UserStoryPropositionEntity>(uspList);

            }
        }
    }
    
    private string GenerateUniqueId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // The character set to use
        var random = new Random();
        string id;

        // Generate a 10 character long string by selecting random characters from the character set
        id = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        
        // Keep generating new strings until we find one that is not already in the SessionList
        //while (SessionList.Sessions.Find(s => s.Identifier.Equals(id)) != new Session());

        return id;
    }

    // public async Task<NoteEntity> createNote(int userID, int cardNumber)
    // {
    //     Session session = _currentSession;
    //     if (session == null)
    //     {
    //         return null;
    //     }
    //
    //     // no more user story to discuss (the session should end)
    //     if (session.currentUserStoryDiscussed() == null)
    //     {
    //         return null;
    //     }
    //     
    //     int usID = _currentSession.currentUserStoryDiscussed().id;
    //
    //     // this user has already set a note for the user story currently discussed
    //     if (_databaseContext.Notes.SingleOrDefault(n =>
    //             n.UserStoryPropositionEntity.id == usID && n.UserEntity.id == userID) != null)
    //     {
    //         return null;
    //     }
    //
    //     NoteEntity noteToSave = new NoteEntity();
    //     noteToSave.note = cardNumber;
    //     noteToSave.UserEntity = _databaseContext.Users.Find(userID);
    //     noteToSave.UserStoryPropositionEntity = _databaseContext.UserStoriesProposition.Find(usID);
    //
    //     _databaseContext.Notes.Add(noteToSave);
    //     await _databaseContext.SaveChangesAsync();
    //
    //     return noteToSave;
    // }

    public async Task<bool> userReadySession(int userID, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return false;
        }

        bool ans = session.userReady(userID);

        // session is now in voting state, so we can push the user stories in it
        // if (session._state is VotingState)
        // {
        //     var allUS = _databaseContext.UserStoriesProposition.ToList();
        //     allUS.Reverse();
        //     session.setAllUserStories(allUS);
        // }

        return ans;  
    }

    public async Task<bool> userNotReadySession(int userID, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return false;
        }

        bool ans = session.userNotReady(userID);

        return ans;
    }

    public async Task<bool> voteForCurrentUS(int userID, int cardNumber, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return false;
        }

        bool ans = session.userVoted(userID, cardNumber);

        // all users have voted the same card
        if (session._CanSaveCurrentUS)
        {
            await storeCurrentUS(session);
        }

        return ans;
    }

    private async Task storeCurrentUS(Session session)
    {
        var currentUS = session.currentUserStoryDiscussed();

        // store in UserStory table
        // get the first value in dictionary
        int[] values = new int[session._currentUSVoted.Count];
        session._currentUSVoted.Values.CopyTo(values, 0);
        int cost = values[0];
        
        UserStoryInput userStoryToAdd = new UserStoryInput
            { description = currentUS.description, estimatedCost = cost, tasks = currentUS.tasks };
        UserStoryEntity us = await _userStoryService.CreateUserStoryAsync(userStoryToAdd);
        
        // store the final us in the session in order to export a .csv file if needed
        session._allVotedUserStories.Push(us);
        
        // delete the proposition because we can now store in UserStory table
        await _userStoryPropositionService.delete(currentUS.id);
        
        // saving changes
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<UserStoryPropositionEntity> createUserStoryProposition(UserStoryPropositionInput usInput, string sessionIdentifier)
    {
        var us = await _userStoryPropositionService.create(usInput);
        
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session != null && us != null)
        {
            session._allUserStories.Push(us);
        }

        
        return us;
    }

    public async Task addWS(WebSocket webSocket, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return;
        }

        session._WebSockets.Add(webSocket);
        await sendUSToAllWS(session.Identifier);
    }

    public void removeWS(WebSocket webSocket, string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return;
        }

        session._WebSockets.Remove(webSocket);
    }

    public async Task sendSessionToAllWS(string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return;
        }
        
        await session.sendSessionToAllWS();
    }

    public async Task sendUSToAllWS(string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return;
        }
        
        var payload = new { type = "userStoriesProposition", userStoriesProposition = session._allUserStories };
        string json = JsonSerializer.Serialize(payload);
        byte[] data = Encoding.UTF8.GetBytes(json);
        
        foreach (WebSocket ws in session._WebSockets)
        {
            await ws.SendAsync(
                new ArraySegment<byte>(data, 0, data.Length),
                WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage,
                CancellationToken.None);
        }
    }

    public async Task showVotesOfEveryone(string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return;
        }

        if (!(session._state is VotingState))
        {
            return;
        }
        
        // so the frontend know it must display all user votes
        session.setState(new DiscussingState(session));
        
        // to go back to voting state after x seconds 
        session._state.onDiscussing();

        await session.sendSessionToAllWS();
    }

    public CustomFile downloadCSV(string sessionIdentifier)
    {
        Session? session = SessionList.Sessions.Find(s => s.Identifier.Equals(sessionIdentifier));

        if (session == null)
        {
            return null;
        }

        List<UserStoryEntity> data = new List<UserStoryEntity>(session._allVotedUserStories);
        
        // Create a memory stream to write the CSV data
    using (var memoryStream = new MemoryStream())
    {
        using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(data);
        }

        var file = new CustomFile
            { FileContents = memoryStream.ToArray(), FileName = "data.csv", ContentType = "text/csv" };

        // Return the file as a FileStreamResult
        return file;
    }
    }
    
}