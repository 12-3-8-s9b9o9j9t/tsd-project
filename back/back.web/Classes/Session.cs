using System.Collections;
using System.Collections.Specialized;
using back.Classes.SessionState;
using back.Entities;
using back.Controllers;
using back.Services;

namespace back.Classes;

public class Session
{
    public ASessionState _state { get; set; }
    public HashSet<int> _joinedUsersID { get; set; }
    
    public Stack<UserStoryPropositionEntity> _allUserStories { get; set; }
    
    public OrderedDictionary _startSessionMap { get; set; }
    
    public OrderedDictionary _currentUSVoted { get; set; }
    
    public OrderedDictionary _currentUSMapValidated { get; set; }

    private static Session _instance;
    
    private Session()
    {
        _joinedUsersID = new HashSet<int>();
        _allUserStories = new Stack<UserStoryPropositionEntity>();
        _startSessionMap = new OrderedDictionary();
        _currentUSMapValidated = new OrderedDictionary();
        _currentUSVoted = new OrderedDictionary();
        _state = new StartState(this);
    }

    public UserStoryPropositionEntity currentUserStoryDiscussed()
    {
        if (_allUserStories.Count == 0)
        {
            return null;
        }

        return _allUserStories.Peek();
    }

    public void nextUserStory()
    {
        _allUserStories.Pop();
        for (int i = 0; i < _currentUSMapValidated.Count; i++)
        {
            _currentUSMapValidated[i] = false;
        }
    }

    public static Session getInstance()
    {
        return _instance;
    }

    public static void createInstance(List<UserStoryPropositionEntity> allUS)
    {
        _instance = new Session();
        _instance._allUserStories = new Stack<UserStoryPropositionEntity>(allUS);
    }

    public void addUser(int id)
    {
        _joinedUsersID.Add(id);
        _startSessionMap.Add(id, false);
        _currentUSVoted.Add(id, -1);
        _currentUSMapValidated.Add(id, -1);
    }

    public bool userStart(int userID)
    {
        if (!_startSessionMap.Contains(userID))
        {
            return false;
        }

        _startSessionMap[(Object)userID] = true;
        _state.onUserStart();
        return true;
    }

    public bool userVoted(int userID, int note)
    {
        if (!_currentUSVoted.Contains(userID))
        {
            return false;
        }
        
        _currentUSVoted[(Object)userID] = note;

        foreach (DictionaryEntry entry in _currentUSVoted)
        {
            Console.WriteLine(entry.Key + " : " + entry.Value);
        }
        

        _state.onUserVote();
        return true;
    }

    public bool userValidated(int userID, int note)
    {
        if (!_currentUSMapValidated.Contains(userID))
        {
            return false;
        }

        _currentUSMapValidated[(Object) userID] = note;
        _state.onUserValidate();
        return true;
    }

    public void setState(ASessionState state)
    {
        _state = state;
    }

    public void resetCurrentUSVoted()
    {
        _currentUSVoted.Clear();

        foreach (int userID in _joinedUsersID)
        {
            _currentUSVoted.Add(userID, -1);
        }
    }

    public void resetCurrentUSValidated()
    {
        _currentUSMapValidated.Clear();

        foreach (int userID in _joinedUsersID)
        {
            _currentUSMapValidated.Add(userID, -1);
        }
    }
}

public class SessionDTO
{
    public List<int> users { get; set; }
    
    public UserStoryPropositionEntity currentUserStory { get; set; }
    
    public OrderedDictionary usersNotes { get; set; }
    
    public string state { get; set; }
}