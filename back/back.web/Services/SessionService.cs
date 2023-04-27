using System.Collections.Specialized;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using back.Classes;
using back.Classes.SessionState;
using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Components;

namespace back.Services;

public interface ISessionService
{
    public Task<SessionDTO> getCurrentSession();

    public Task<bool> addUserToSession(int id);

    public SessionDTO createSession();

    // public Task<NoteEntity> createNote(int userID, int cardNumber);

    public Task<bool> voteForCurrentUS(int userID, int cardNumber);
    
    public Task<bool> userStartSession(int userID);

    public Task<UserStoryPropositionEntity> createUserStoryProposition(UserStoryPropositionInput usInput);

    public void addWS(WebSocket webSocket);

    public void removeWS(WebSocket webSocket);

    public Task sendSessionToAllWS();

    public Task sendUSToAllWS();
}

public class SessionService : ISessionService
{
    private readonly IUserStoryPropositionService _userStoryPropositionService;

    private readonly IUserStoryService _userStoryService;

    private readonly DatabaseContext _databaseContext;

    private readonly IUserService _userService;
    
    private Session _currentSession { get; set; }

    public SessionService(IUserStoryPropositionService userStoryPropositionService, IUserStoryService userStoryService,DatabaseContext databaseContext, IUserService userService)
    {
        _userStoryPropositionService = userStoryPropositionService;
        _userStoryService = userStoryService;
        _currentSession = Session.getInstance();
        _databaseContext = databaseContext;
        _userService = userService;
    }

    public async Task<SessionDTO> getCurrentSession()
    {
        if (_currentSession == null)
        {
            return null;
        }

        if (_currentSession._joinedUsers == null || _currentSession._allUserStories == null)
        {
            return null;
        }
        
        var sessionDTO = new SessionDTO();
        sessionDTO.currentUserStory = _currentSession.currentUserStoryDiscussed();
        var users = _currentSession._joinedUsers;
        sessionDTO.users = new List<UserDTO>(users.Select(user => new UserDTO { id = user.id, name = user.name}));
        sessionDTO.state = _currentSession._state.ToString();
        sessionDTO.usersNotes = _currentSession._state.getUsersVote();
        sessionDTO.nb_ws = _currentSession._WebSockets.Count;
        return sessionDTO;
    }

    public async Task<bool> addUserToSession(int id)
    {
        if (_currentSession == null)
        {
            return false;
        }
        
        // if user does not exist
        if (await _userService.GetByID(id) == null)
        {
            return false;
        }
        
        // if user already in session
        if (_currentSession._joinedUsers.Select(user => user.id).Contains(id))
        {
            return false;
        }

        UserDTO user = await _userService.GetByID(id);
        _currentSession.addUser(new UserEntity { id = user.id, name = user.name});
        
        return true;
    }

    public SessionDTO createSession()
    {
        Session.createInstance();
        _currentSession = Session.getInstance();

        SessionDTO result = new SessionDTO();
        var users = _currentSession._joinedUsers;
        result.users = new List<UserDTO>(users.Select(user => new UserDTO { id = user.id, name = user.name }));
        result.currentUserStory = _currentSession.currentUserStoryDiscussed();
        result.state = _currentSession._state.ToString();
        result.usersNotes = null;

        return result; 
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

    public async Task<bool> userStartSession(int userID)
    {
        bool ans = _currentSession.userStart(userID);

        // session is now in voting state, so we can push the user stories in it
        if (_currentSession._state is VotingState)
        {
            var allUS = _databaseContext.UserStoriesProposition.ToList();
            allUS.Reverse();
            _currentSession.setAllUserStories(allUS);
        }

        return ans;  
    }

    public async Task<bool> voteForCurrentUS(int userID, int cardNumber)
    {
        bool ans = _currentSession.userVoted(userID, cardNumber);

        // all users have voted the same card
        if (_currentSession._CanSaveCurrentUS)
        {
            await sendSessionToAllWS();
            await storeCurrentUS();
        }

        return ans;
    }

    private async Task storeCurrentUS()
    {
        var currentUS = _currentSession.currentUserStoryDiscussed();

        // store in UserStory table
        // get the first value in dictionary
        int[] values = new int[_currentSession._currentUSVoted.Count];
        _currentSession._currentUSVoted.Values.CopyTo(values, 0);
        int cost = values[0];
        
        UserStoryInput userStoryToAdd = new UserStoryInput
            { description = currentUS.description, estimatedCost = cost };
        await _userStoryService.CreateUserStoryAsync(userStoryToAdd);
        
        // delete the proposition because we can now store in UserStory table
        await _userStoryPropositionService.delete(currentUS.id);
        
        // saving changes
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<UserStoryPropositionEntity> createUserStoryProposition(UserStoryPropositionInput usInput)
    {
        var us = await _userStoryPropositionService.create(usInput);

        return us.Value;
    }

    public void addWS(WebSocket webSocket)
    {
        if (_currentSession == null)
        {
            return;
        }
        
        _currentSession._WebSockets.Add(webSocket);
    }

    public void removeWS(WebSocket webSocket)
    {
        if (_currentSession == null)
        {
            return;
        }

        _currentSession._WebSockets.Remove(webSocket);
    }

    public async Task sendSessionToAllWS()
    {
        await _currentSession.sendSessionToAllWS();
    }

    public async Task sendUSToAllWS()
    {
        var payload = new { type = "userStoriesProposition", userStoriesProposition = _userStoryPropositionService.getAll() };
        string json = JsonSerializer.Serialize(payload);
        byte[] data = Encoding.UTF8.GetBytes(json);
        
        foreach (WebSocket ws in _currentSession._WebSockets)
        {
            await ws.SendAsync(
                new ArraySegment<byte>(data, 0, data.Length),
                WebSocketMessageType.Text,
                WebSocketMessageFlags.EndOfMessage,
                CancellationToken.None);
        }
    }
}