using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserStoryController : ControllerBase
{
    private readonly UserStoryContext _userStoryContext;

    public UserStoryController(UserStoryContext userStoryContext)
    {
        _userStoryContext = userStoryContext;
    }

    [HttpGet("all")]
    public IEnumerable<UserStoryEntity> get()
    {
        return _userStoryContext.UserStories.OrderBy(u => u.id);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStoryEntity>> getByID(int id)
    {
        var userStory = await _userStoryContext.UserStories.FindAsync(id);

        if (userStory == null)
        {
            return NotFound();
        }

        return userStory;
    }

    [HttpPost]
    public async Task<ActionResult<UserStoryEntity>> create(UserStoryInput userStory)
    {
        UserStoryEntity userStoryToAdd = new UserStoryEntity(userStory.description, userStory.estimatedCost);
        _userStoryContext.UserStories.Add(userStoryToAdd);
        await _userStoryContext.SaveChangesAsync();

        return userStoryToAdd;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserStoryEntity>> delete(int id)
    {

        var userStory = await getByID(id);

        _userStoryContext.UserStories.Remove(userStory.Value);
        await _userStoryContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserStoryEntity>> update(int id, [FromBody] UserStoryInput userStory)
    {
        var u = await getByID(id);

        if (u.Value == null)
        {
            return NotFound();
        }

        if (userStory.description != null)
        {
            u.Value.description = new string(userStory.description);
        }

        if (userStory.estimatedCost != null)
        {
            u.Value.estimatedCost = userStory.estimatedCost;
        }

        await _userStoryContext.SaveChangesAsync();
        return Ok(u.Value);
    }
}