using System.Collections;
using System.Collections.Specialized;

namespace back.Classes.SessionState;

public class DiscussingState : ASessionState
{
    public DiscussingState(Session session) : base(session)
    {
    }

    public override void onUserVote()
    {
        // nothing to do
    }

    public override void onDiscussing()
    {
        bool sameRes = true; 
    
        // get the first value in dictionary
        int[] values = new int[_session._currentUSVoted.Count];
        _session._currentUSVoted.Values.CopyTo(values, 0);
        int firstVote = values[0];

        foreach (DictionaryEntry entry in _session._currentUSVoted)
        {
            if ((int) entry.Value != firstVote)
            {
                sameRes = false;    
                break;
            }
        }

        // in order to know if we can store in db the us
        _session._CanSaveCurrentUS = sameRes;

        Task.Delay(5000).ContinueWith(_ =>
        {
            _session._CanSaveCurrentUS = false;
            // OK, we can vote the next user story
            if (sameRes)
            {
                _session.nextUserStory();
            }
            _session.resetCurrentUSVoted();
            
            // if current users story is null => go to end state
            // else go to voting state
            if (_session.currentUserStoryDiscussed() == null)
            {
                _session.setState(new EndState(_session));
            }
            else
            {
                _session.setState(new VotingState(_session));
            }
        });
    }

    public override void onUserStart()
    {
        // nothing to do
    }

    public override OrderedDictionary getUsersVote()
    {
        return _session._currentUSVoted;
    }

    public override string ToString()
    {
        return "discussing";
    }
}