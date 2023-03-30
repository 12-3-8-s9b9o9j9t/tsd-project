using System.Collections.Specialized;

namespace back.Classes.SessionState;

public abstract class ASessionState
{
    protected Session _session;

    protected ASessionState(Session session)
    {
        _session = session;
    }

    public abstract void onUserVote();

    public abstract void onUserValidate();

    public abstract void onUserStart();

    public abstract OrderedDictionary getUsersVote(); // to get all users vote depending on the state (voting or discussing)

    public abstract override string ToString();
}