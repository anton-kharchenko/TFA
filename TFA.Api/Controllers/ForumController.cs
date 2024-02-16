using Microsoft.AspNetCore.Mvc;
using TFA.Api.Requests;
using TFA.Api.Responses;
using TFA.Domain.Commands.CreateTopic;
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
    public Task<IActionResult> GetForums()
    {
        return Task.FromResult<IActionResult>(Ok());
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
        var command = new CreateTopicCommand(forumId, request.Title);

        var topic = await topicUseCase.ExecuteAsync(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), new TopicResponse
        {
            Id = topic.Id,
            Title = topic.Title,
            CreatedAt = topic.CreatedAt
        });
    }
}