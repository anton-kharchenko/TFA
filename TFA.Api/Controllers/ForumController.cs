using Microsoft.AspNetCore.Mvc;

namespace TFA.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetForums()
    {
        return Ok();
    }
}

