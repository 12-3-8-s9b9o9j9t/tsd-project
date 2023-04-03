using System.Collections;
using System.Collections.Specialized;

namespace back.Classes.SessionState;

public class VotingState : ASessionState
{
    public VotingState(Session session) : base(session)
    {
    }

    public override void onUserVote()
    {
        foreach (DictionaryEntry entry in _session._currentUSVoted)
        {
            // one or several developer have not voted the current user story yet
            if ((int) entry.Value < 0)
            {
                return;
            }
        }

        //_session.resetCurrentUSVoted();
        _session.setState(new DiscussingState(_session));
        _session.startDiscussing();
        Console.WriteLine("all dev have voted");
    }

    public override void onDiscussing()
    {
        // nothing to do
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
        return "voting";
    }
}