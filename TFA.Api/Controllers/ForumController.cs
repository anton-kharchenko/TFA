using Microsoft.AspNetCore.Mvc;
using TFA.Api.Models;

namespace TFA.Api.Controllers;

[ApiController]
[Route("forums")]
public class ForumController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Forum))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetForums()
    {
        
        return Ok();
    }
}

