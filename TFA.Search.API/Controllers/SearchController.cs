using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Search.Domain.Commands;
using TFA.Search.Domain.Models;
using TFA.Search.Domain.Queries;

namespace TFA.Search.API.Controllers;

public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpPost("index")]
    public async Task<IActionResult> Index([FromBody] SearchEntity searchEntity, CancellationToken cancellationToken)
    {
        var command = new IndexCommand(searchEntity.EntityId, searchEntity.SearchEntityType, searchEntity.Title,
            searchEntity.Text!);

        await mediator.Send(command, cancellationToken);

        return Ok();
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search(string query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SearchQuery(query), cancellationToken);
       
        return Ok(result);
    }
}