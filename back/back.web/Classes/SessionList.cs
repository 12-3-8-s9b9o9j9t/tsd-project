namespace back.Classes;

public class SessionList
{
    public static List<Session>? Sessions;

    public static void Add(Session session)
    {
        if (Sessions == null)
        {
            Sessions = new List<Session>();
        }
        
        Sessions.Add(session);
    }
}