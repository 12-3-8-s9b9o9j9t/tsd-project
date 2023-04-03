using back.DAL;
using back.Entities;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> Create([FromBody] UserInput userInput)
    {
        var createdUser = await _userService.CreateUserAsync(userInput);
        return Ok(createdUser);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<UserDTO>> GetByName(string name)
    {
        var user = await _userService.GetUserByNameAsync(name);
        if (user == null)
        {
            return NotFound($"User with name '{name}' not found.");
        }
        return Ok(user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDTO>> GetByID(int id)
    {
        var user = await _userService.GetByID(id);
        if (user == null)
        {
            return NotFound($"User with id '{id}' not found.");
        }
        return Ok(user);
    }

}