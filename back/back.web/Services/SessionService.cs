using System.Collections.Specialized;
using back.Classes;
using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Components;

namespace back.Services;

public interface ISessionService
{
    public Task<SessionDTO> getCurrentSession();

    public bool addUserToSession(int id);

    public SessionDTO createSession();

    public Task<NoteEntity> createNote(int userID, int cardNumber);

    public bool voteForCurrentUS(int userID, int cardNumber);
    
    public bool userStartSession(int userID);
}

public class SessionService : ISessionService
{
    private readonly IUserStoryPropositionService _userStoryPropositionService;

    private readonly DatabaseContext _databaseContext;

    private readonly IUserService _userService;
    
    private Session _currentSession { get; set; }

    public SessionService(IUserStoryPropositionService userStoryPropositionService, DatabaseContext databaseContext, IUserService userService)
    {
        _userStoryPropositionService = userStoryPropositionService;
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

        if (_currentSession._joinedUsersID == null || _currentSession._allUserStories == null)
        {
            return null;
        }
        
        var sessionDTO = new SessionDTO();
        sessionDTO.currentUserStory = _currentSession.currentUserStoryDiscussed();
        var users = _currentSession._joinedUsersID.Select(id => _userService.GetByID(id).Result);
        sessionDTO.users = new List<UserDTO>(users);
        sessionDTO.state = _currentSession._state.ToString();
        sessionDTO.usersNotes = _currentSession._state.getUsersVote();
        return sessionDTO;
    }

    public bool addUserToSession(int id)
    {
        if (_currentSession == null)
        {
            return false;
        }
        
        // if user does not exist
        if (_databaseContext.Users.Find(id) == null)
        {
            return false;
        }
        
        // if user already in session
        if (_currentSession._joinedUsersID.Contains(id))
        {
            return false;
        }
        
        _currentSession.addUser(id);
        return true;
    }

    public SessionDTO createSession()
    {
        var allUS = _userStoryPropositionService.getAll().OrderByDescending(u => u.id);
        Session.createInstance(new List<UserStoryPropositionEntity>(allUS));
        _currentSession = Session.getInstance();

        SessionDTO result = new SessionDTO();
        var users = _currentSession._joinedUsersID.Select(id => _userService.GetByID(id).Result);
        result.users = new List<UserDTO>(users);
        result.currentUserStory = _currentSession.currentUserStoryDiscussed();
        result.state = _currentSession._state.ToString();
        result.usersNotes = null;

        return result; 
    }

    public async Task<NoteEntity> createNote(int userID, int cardNumber)
    {
        Session session = _currentSession;
        if (session == null)
        {
            return null;
        }

        // no more user story to discuss (the session should end)
        if (session.currentUserStoryDiscussed() == null)
        {
            return null;
        }
        
        int usID = _currentSession.currentUserStoryDiscussed().id;

        // this user has already set a note for the user story currently discussed
        if (_databaseContext.Notes.SingleOrDefault(n =>
                n.UserStoryPropositionEntity.id == usID && n.UserEntity.id == userID) != null)
        {
            return null;
        }

        NoteEntity noteToSave = new NoteEntity();
        noteToSave.note = cardNumber;
        noteToSave.UserEntity = _databaseContext.Users.Find(userID);
        noteToSave.UserStoryPropositionEntity = _databaseContext.UserStoriesProposition.Find(usID);

        _databaseContext.Notes.Add(noteToSave);
        await _databaseContext.SaveChangesAsync();

        return noteToSave;
    }

    public bool userStartSession(int userID)
    {
        return _currentSession.userStart(userID);
    }

    public bool voteForCurrentUS(int userID, int cardNumber)
    {
        return _currentSession.userVoted(userID, cardNumber);
    }
}