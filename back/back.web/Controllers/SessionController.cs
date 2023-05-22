using System.Net.WebSockets;
using back.Classes;
using back.Entities;
using back.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionController : ControllerBase
{

    private readonly ISessionService _sessionService;

    private readonly IUserService _userService;

    public SessionController(ISessionService sessionService, IUserService userService)
    {
        _sessionService = sessionService;
        _userService = userService;
    }

    [HttpGet("{sessionIdentifier}")]
    public async Task<ActionResult<Session>> get(string sessionIdentifier)
    {
        var result = await _sessionService.getSession(sessionIdentifier);

        if (result == null)
        {
            return BadRequest("session is null.");
        }
        return Ok(result);
    }

    [HttpPost("addUser/{id:int}/{sessionIdentifier}")]
    public async Task<ActionResult<Session>> addUser(int id, string sessionIdentifier)
    {
        bool result = await _sessionService.addUserToSession(id, sessionIdentifier);

        if (!result)
        {
            return BadRequest("User " + id + " already in current session or current session is null or does not exist");
        }

        await _sessionService.sendSessionToAllWS(sessionIdentifier);

        return Ok(await _sessionService.getSession(sessionIdentifier));
    }

    [HttpPost("createSession")]
    public async Task<ActionResult<Session>> createSession(IFormFile? jiraFile)
    {
        var result = _sessionService.createSession(jiraFile);
        
        return Ok(result);
    }

    [HttpPost("ready/{userID:int}/{sessionIdentifier}")]
    public async Task<ActionResult<Session>> userStart(int userID, string sessionIdentifier)
    {
        var currentSession = await _sessionService.getSession(sessionIdentifier);
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        var usersIDList = currentSession.users.Select(u => u.id);

        if (!usersIDList.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = await _sessionService.userReadySession(userID, sessionIdentifier);

        if (!result)
        {
            return BadRequest("bad start");
        }
        
        await _sessionService.sendSessionToAllWS(sessionIdentifier);

        return Ok(await _sessionService.getSession(sessionIdentifier));

    }
    
    [HttpPost("notready/{userID:int}/{sessionIdentifier}")]
    public async Task<ActionResult<Session>> userNotReady(int userID, string sessionIdentifier)
    {
        var currentSession = await _sessionService.getSession(sessionIdentifier);
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        var usersIDList = currentSession.users.Select(u => u.id);

        if (!usersIDList.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = await _sessionService.userNotReadySession(userID, sessionIdentifier);

        if (!result)
        {
            return BadRequest("bad start");
        }
        
        await _sessionService.sendSessionToAllWS(sessionIdentifier);

        return Ok(await _sessionService.getSession(sessionIdentifier));

    }

    [HttpPost("voteCurrentUserStory/{userID:int}/{cardNumber:int}/{sessionIdentifier}")]
    public async Task<ActionResult<Session>> voteForCurrentUS(int userID, int cardNumber, string sessionIdentifier)
    {
        Console.WriteLine("user id : " + userID + " card : " + cardNumber);
        var currentSession = await _sessionService.getSession(sessionIdentifier);
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }
        
        var usersIDList = currentSession.users.Select(u => u.id);

        if (!usersIDList.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = await _sessionService.voteForCurrentUS(userID, cardNumber, sessionIdentifier);

        if (!result)
        {
            return BadRequest("note " + cardNumber + " for user " + userID + " for this user story already exist");
        }
        
        // comment because we send the session once when all the users have voted
        // await _sessionService.sendSessionToAllWS();
        
        return Ok(await _sessionService.getSession(sessionIdentifier));
        
    }

    [HttpPost("createUserStoryProposition/{sessionIdentifier}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> createUserStoryProposition([FromBody] UserStoryPropositionInput usInput, string sessionIdentifier)
    {
        var us = await _sessionService.createUserStoryProposition(usInput, sessionIdentifier);
        await _sessionService.sendUSToAllWS(sessionIdentifier);
        return Ok(us);
    }

    [HttpGet("showVotesOfEveryone/{sessionIdentifier}")]
    public async Task<ActionResult> showVotesOfEveryone(string sessionIdentifier)
    {
        await _sessionService.showVotesOfEveryone(sessionIdentifier);
        return Ok();
    }
}









 