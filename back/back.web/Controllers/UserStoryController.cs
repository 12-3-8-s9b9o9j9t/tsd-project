using back.DAL;
using Microsoft.AspNetCore.Mvc;

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


}