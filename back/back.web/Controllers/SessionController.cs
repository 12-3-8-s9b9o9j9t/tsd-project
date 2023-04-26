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

    [HttpGet]
    public async Task<ActionResult<Session>> get()
    {
        var result = await _sessionService.getCurrentSession();

        if (result == null)
        {
            return BadRequest("current session is null.");
        }
        return Ok(result);
    }

    [HttpPost("addUser/{id:int}")]
    public async Task<ActionResult<Session>> addUser(int id)
    {
        bool result = await _sessionService.addUserToSession(id);

        if (!result)
        {
            return BadRequest("User " + id + " already in current session or current session is null or does not exist");
        }

        await _sessionService.sendSessionToAllWS();

        return Ok(await _sessionService.getCurrentSession());
    }

    [HttpPost("createSession")]
    public async Task<ActionResult<Session>> createSession()
    {
        _sessionService.createSession();
        
        return Ok(await _sessionService.getCurrentSession());
    }

    [HttpPost("start/{userID:int}")]
    public async Task<ActionResult<Session>> userStart(int userID)
    {
        var currentSession = await _sessionService.getCurrentSession();
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        var usersIDList = currentSession.users.Select(u => u.Id);

        if (!usersIDList.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = await _sessionService.userStartSession(userID);

        if (!result)
        {
            return BadRequest("bad start");
        }
        
        await _sessionService.sendSessionToAllWS();

        return Ok(await _sessionService.getCurrentSession());

    }

    [HttpPost("voteCurrentUserStory/{userID:int}/{cardNumber:int}")]
    public async Task<ActionResult<Session>> voteForCurrentUS(int userID, int cardNumber)
    {
        Console.WriteLine("user id : " + userID + " card : " + cardNumber);
        var currentSession = await _sessionService.getCurrentSession();
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }
        
        var usersIDList = currentSession.users.Select(u => u.Id);

        if (!usersIDList.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = await _sessionService.voteForCurrentUS(userID, cardNumber);

        if (!result)
        {
            return BadRequest("note " + cardNumber + " for user " + userID + " for this user story already exist");
        }
        
        await _sessionService.sendSessionToAllWS();
        
        return Ok(await _sessionService.getCurrentSession());
        
    }
}









 