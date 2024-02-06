using Microsoft.AspNetCore.Mvc;
using TFA.Api.Requests;
using TFA.Api.Responses;
using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.UseCases.CreateTopic;
using Forum = TFA.Api.Models.Forum;

namespace TFA.Api.Controllers;

[ApiController]
[Route("forums")]
public class ForumController : ControllerBase
{
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(Forum))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetForums()
    {
        
        return Ok();
    }

    [HttpPost("{forumId}/topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(TopicResponse))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId, 
        [FromBody] CreateTopicRequest request,
        [FromServices] ICreateTopicUseCase topicUseCase, 
        CancellationToken cancellationToken)
    {
        try
        {
            var topic = await topicUseCase.ExecuteAsync(forumId, request.Title, cancellationToken);
            
            return CreatedAtRoute(nameof(GetForums), new TopicResponse
            {
                Id = topic.Id,
                Title = topic.Title,
                CreatedAt = topic.CreatedAt
            });
        }
        catch (Exception exception)
        {
            return exception switch
            {
                IntentionManagerException => Forbid(),
                ForumNotFoundException => StatusCode(StatusCodes.Status410Gone),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
    
}

