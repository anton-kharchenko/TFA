using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TFA.Api.Requests.Forum;
using TFA.Api.Requests.Topic;
using TFA.Api.Responses;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Interfaces.UseCases.CreateTopic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;

namespace TFA.Api.Controllers;

[ApiController]
[Route("forums")]
public class ForumController : ControllerBase
{
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(ForumResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetForums(
        [FromServices] IGetForumsUseCase useCase,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var forums = await useCase.ExecuteAsync(cancellationToken);
        return Ok(forums!.Select(mapper.Map<ForumResponse>));
    }

    [HttpPost("{forumId}/topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(TopicResponse))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopicRequest request,
        [FromServices] ICreateTopicUseCase topicUseCase,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = new CreateTopicCommand(forumId, request.Title);

        var topic = await topicUseCase.ExecuteAsync(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), mapper.Map<TopicResponse>(topic));
    }

    [HttpGet("{forumId:guid}/topics")]
    public async Task<IActionResult> GetTopic(
        [FromRoute] Guid forumId,
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromServices] IGetTopicsUseCase topicsUseCase,
        CancellationToken cancellationToken)
    {
        var query = new GetTopicsQuery(forumId, skip, take);

        var (resources, totalCount) = await topicsUseCase.ExecuteAsync(query, cancellationToken);

        return Ok(new { resources, totalCount });
    }

    [HttpPost]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(ForumResponse))]
    public async Task<IActionResult> CreateForum(
        [FromBody] CreateForumRequest request,
        [FromServices] ICreateForumUseCase useCase,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = new CreateForumCommand(request.Title);

        var forum = await useCase.ExecuteAsync(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), mapper.Map<ForumResponse>(forum));
    }
}