using back.DAL;
using back.Entities;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("[controller]")]
public class UserStoryPropositionController : ControllerBase
{
    private readonly IUserStoryPropositionService _service;

    public UserStoryPropositionController(IUserStoryPropositionService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<UserStoryPropositionEntity> get()
    {
        return _service.getAll();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> getByID(int id)
    {
        var userStoryP = await _service.getByID(id);

        if (userStoryP == null)
        {
            return NotFound();
        }

        return userStoryP;
    }

    [HttpPost]
    public async Task<ActionResult<UserStoryPropositionEntity>> create([FromBody] UserStoryPropositionInput userStoryP)
    {
        return await _service.create(userStoryP);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> delete(int id)
    {
        bool ans = await _service.delete(id);
        return ans ? Ok() : NotFound();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserStoryPropositionEntity>> update(int id, [FromBody] UserStoryPropositionInput input)
    {
        var ans = _service.update(id, input);

        if (ans == null)
        {
            return NotFound();
        }
        return Ok(ans);
    }
}






