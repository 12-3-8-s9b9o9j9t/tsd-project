using back.DAL;
using back.Entities;
using back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserStoryController : ControllerBase
{
    private readonly IUserStoryService _userStoryService;

    public UserStoryController(IUserStoryService userStoryService)
    {
        _userStoryService = userStoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserStoryEntity>>> GetAllUserStories()
    {
        var userStories = await _userStoryService.GetAllUserStoriesAsync();

        return Ok(userStories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStoryEntity>> GetUserStoryById(int id)
    {
        var userStory = await _userStoryService.GetUserStoryByIdAsync(id);

        if (userStory == null)
        {
            return NotFound();
        }

        return Ok(userStory);
    }

    [HttpPost]
    public async Task<ActionResult<UserStoryEntity>> CreateUserStory(UserStoryInput userStory)
    {
        var createdUserStory = await _userStoryService.CreateUserStoryAsync(userStory);

        return CreatedAtAction(nameof(GetUserStoryById), new { id = createdUserStory.id }, createdUserStory);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserStory(int id)
    {
        var deletedUserStory = await _userStoryService.DeleteUserStoryAsync(id);

        if (deletedUserStory == null)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUserStory(int id, UserStoryInput userStory)
    {
        var updatedUserStory = await _userStoryService.UpdateUserStoryAsync(id, userStory);

        if (updatedUserStory == null)
        {
            return NotFound();
        }

        return Ok(updatedUserStory);
    }
}