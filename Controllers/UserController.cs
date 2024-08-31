using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Api.Controllers;

[Authorize(Roles = "User")]
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    [Route("~/api/[controller]")]
    [HttpGet]
    public ActionResult Get()
    {
        return Ok("You have accessed the User controller.");
    }
}
