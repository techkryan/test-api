using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicCatalog.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]/[action]")]
[ApiController]
public class AdminController : ControllerBase
{
    [Route("~/api/[controller]")]
    [HttpGet]
    public ActionResult Get()
    {
        return Ok("You have accessed the Admin controller.");
    }
}
