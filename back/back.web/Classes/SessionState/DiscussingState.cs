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
        _session._currentUSMapValidated.Values.CopyTo(values, 0);
        int firstVote = values[0];

        foreach (DictionaryEntry entry in _session._currentUSVoted)
        {
            if ((int) entry.Value != firstVote)
            {
                sameRes = false;    
                break;
            }
        }
        
        Console.WriteLine("on discussing");
        // OK, we can go to next user story
        if (sameRes)
        {
            Task.Delay(5000).ContinueWith(_ =>
            {
                _session.resetCurrentUSVoted();
                _session.setState(new VotingState(_session));
                _session.nextUserStory();
            });
        }
        // return to vote state
        else
        {
            Task.Delay(5000).ContinueWith(_ =>
            {
                _session.resetCurrentUSVoted();
                _session.setState(new VotingState(_session));
            });


        }
    }

    // public override void onUserValidate()
    // {
    //     if (_session._currentUSMapValidated.Count == 0)
    //     {
    //         return;
    //     }
    //
    //     // get the first value in dictionary
    //     int[] values = new int[_session._currentUSMapValidated.Count];
    //     _session._currentUSMapValidated.Values.CopyTo(values, 0);
    //     int finalVote = values[0];
    //
    //     foreach (DictionaryEntry entry in _session._currentUSMapValidated)
    //     {
    //         // one or several developer have not validated the current user story discussed yet
    //         if (finalVote != (int) entry.Value || (int) entry.Value < 0)
    //         {
    //             return;
    //         }
    //     }
    //
    //     _session.resetCurrentUSValidated();
    //     _session.nextUserStory();
    //     _session.setState(new VotingState(_session));
    //     Console.WriteLine("all dev have discussed");
    // }

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