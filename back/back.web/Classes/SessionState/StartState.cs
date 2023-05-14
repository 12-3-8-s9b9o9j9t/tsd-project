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

    public override void onDiscussing()
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
        return _session._startSessionMap; // not votes in reality but information about players (if they are ready or not)
    }

    public override string ToString()  
    {
        return "starting";
    }
}