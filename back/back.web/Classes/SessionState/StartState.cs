using System.Collections;
using System.Collections.Specialized;

namespace back.Classes.SessionState;

public class StartState : ASessionState
{
    public StartState(Session session) : base(session)
    {
    }

    public override void onUserVote()
    {
        // nothing to do
    }

    public override void onUserValidate()
    {
        // nothing to do
    }

    public override void onUserStart()
    {
        foreach (DictionaryEntry entry in _session._startSessionMap)
        {
            // switch state only if all users want to start
            if (!(bool) entry.Value)
            {
                return;
            }
        }
        
        _session.setState(new VotingState(_session));
    }

    public override OrderedDictionary getUsersVote()
    {
        return null; // no vote for this state
    }

    public override string ToString()
    {
        return "starting";
    }
}