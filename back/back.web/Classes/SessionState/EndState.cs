using System.Collections.Specialized;

namespace back.Classes.SessionState;

public class EndState : ASessionState
{
    public EndState(Session session) : base(session)
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
        // nothing to do
    }

    public override OrderedDictionary getUsersVote()
    {
        // nothing to do
        return null;
    }

    public override string ToString()
    {
        return "end";
    }
}