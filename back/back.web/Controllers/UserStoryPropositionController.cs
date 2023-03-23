using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserStoryPropositionController : ControllerBase
{
    private readonly UserStoryPropositionContext _userStoryPropositionContext;

    public UserStoryPropositionController(UserStoryPropositionContext context)
    {
        _userStoryPropositionContext = context;
    }

    [HttpGet("all")]
    public IEnumerable<UserStoryPropositionEntity> get()
    {
        return _userStoryPropositionContext.UserStoriesProposition.OrderBy(u => u.id);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> getByID(int id)
    {
        var userStoryP = await _userStoryPropositionContext.UserStoriesProposition.FindAsync(id);

        if (userStoryP == null)
        {
            return NotFound();
        }

        return userStoryP;
    }

    [HttpPost]
    public async Task<ActionResult<UserStoryPropositionEntity>> create([FromBody] UserStoryPropositionInput userStoryP)
    {
        UserStoryPropositionEntity userStoryPropositionToAdd = new UserStoryPropositionEntity(userStoryP.description);
        _userStoryPropositionContext.UserStoriesProposition.Add(userStoryPropositionToAdd);
        await _userStoryPropositionContext.SaveChangesAsync();

        return userStoryPropositionToAdd;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> delete(int id)
    {
        var userStoryPropositionToDelete = await getByID(id);

        _userStoryPropositionContext.UserStoriesProposition.Remove(userStoryPropositionToDelete.Value);
        await _userStoryPropositionContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> update(int id, [FromBody] UserStoryPropositionInput input)
    {
        var userStoryP = getByID(id);

        if (userStoryP == null)
        {
            return NotFound();
        }

        if (input.description != null)
        {
            userStoryP.Result.Value.description = input.description;
        }

        await _userStoryPropositionContext.SaveChangesAsync();

        return Ok(userStoryP);
    }
}






