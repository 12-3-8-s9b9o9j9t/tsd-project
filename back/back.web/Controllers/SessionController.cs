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

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    

    [HttpGet]
    public async Task<ActionResult<Session>> get()
    {
        var result = _sessionService.getCurrentSession();

        if (result == null)
        {
            return BadRequest("current session is null.");
        }
        return Ok(result);
    }

    [HttpPost("addUser/{id:int}")]
    public async Task<ActionResult<Session>> addUser(int id)
    {
        bool result = _sessionService.addUserToSession(id);

        if (!result)
        {
            return BadRequest("User " + id + " already in current session or current session is null or does not exist");
        }

        return Ok(_sessionService.getCurrentSession());
    }

    [HttpPost("createSession")]
    public async Task<ActionResult<Session>> createSession()
    {
        _sessionService.createSession();
        
        return Ok(_sessionService.getCurrentSession());
    }

    [HttpPost("start/{userID:int}")]
    public async Task<ActionResult<Session>> userStart(int userID)
    {
        var currentSession = _sessionService.getCurrentSession();
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        if (!currentSession.users.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = _sessionService.userStartSession(userID);

        if (!result)
        {
            return BadRequest("bad start");
        }

        return Ok(_sessionService.getCurrentSession());

    }

    [HttpPost("voteCurrentUserStory/{userID:int}/{cardNumber:int}")]
    public async Task<ActionResult<Session>> voteForCurrentUS(int userID, int cardNumber)
    {
        Console.WriteLine("user id : " + userID + " card : " + cardNumber);
        var currentSession = _sessionService.getCurrentSession();
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        if (!currentSession.users.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = _sessionService.voteForCurrentUS(userID, cardNumber);

        if (!result)
        {
            return BadRequest("note " + cardNumber + " for user " + userID + " for this user story already exist");
        }
        
        return Ok(_sessionService.getCurrentSession());
        
    }

    [HttpPost("validateCurrentUserStory/{userID:int}/{cardNumber:int}")]
    public async Task<ActionResult<Session>> validateCurrentUS(int userID, int cardNumber)
    {
        var currentSession = _sessionService.getCurrentSession();
        if (currentSession == null)
        {
            return BadRequest("current session is null.");
        }

        if (!currentSession.users.Contains(userID))
        {
            return BadRequest("user " + userID + " is not in current session.");
        }

        bool result = _sessionService.validateForCurrentUS(userID, cardNumber);

        if (!result)
        {
            return BadRequest("note " + cardNumber + " for user " + userID + " for this user story already exist");
        }
        
        return Ok(_sessionService.getCurrentSession());
    }
}









