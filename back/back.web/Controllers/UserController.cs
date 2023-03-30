using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public UserController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public IEnumerable<UserEntity> getAll()
    {
        return _databaseContext.Users.OrderBy(u => u.id);
    }

    [HttpPost]
    public async Task<ActionResult<UserEntity>> create([FromBody] UserInput userInput)
    {
        if (userInput.name.Length == 0 || userInput.name.Contains(' '))
        {
            return BadRequest("name must be valid");
        }

        if (_databaseContext.Users.SingleOrDefault(u => u.name == userInput.name) != null)
        {
            return BadRequest("user " + userInput.name + " already exist");
        }

        UserEntity userToAdd = new UserEntity();
        userToAdd.name = userInput.name;

        _databaseContext.Users.Add(userToAdd);
        await _databaseContext.SaveChangesAsync();

        return Ok(userToAdd);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<UserEntity>> getByName(string name)
    {
        Console.WriteLine("name : " + name);
        if (name.Length == 0)
        {
            return BadRequest("name must be valid");
        }

        var result = _databaseContext.Users.SingleOrDefault(u => u.name.Equals(name));

        if (result == null)
        {
            return NotFound("user " + name + " not found");
        }

        return Ok(result);
    }

}