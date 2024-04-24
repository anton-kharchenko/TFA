using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Forums.Api.Requests.Forum;
using TFA.Forums.Api.Requests.Topic;
using TFA.Forums.Api.Responses;
using TFA.Forums.Domain.Commands.CreateForum;
using TFA.Forums.Domain.Commands.CreateTopic;
using TFA.Forums.Domain.Commands.GetTopics;
using TFA.Forums.Domain.Queries.GetForum;

namespace TFA.Forums.Api.Controllers;

[ApiController]
[Route("forums")]
public class ForumController(ISender sender, IMapperBase mapper) : ControllerBase
{
    [HttpGet(Name = nameof(GetForums))]
    [ProducesResponseType(200, Type = typeof(ForumResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetForums(CancellationToken cancellationToken)
    {
        
        var forums = await sender.Send(new GetForumQuery(),cancellationToken);
        
        return Ok(forums.Select(mapper.Map<ForumResponse>));
    }

    [HttpPost("{forumId}/topics")]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(TopicResponse))]
    public async Task<IActionResult> CreateTopic(
        Guid forumId,
        [FromBody] CreateTopicRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTopicCommand(forumId, request.Title);

        var topic = await sender.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), mapper.Map<TopicResponse>(topic));
    }

    [HttpGet("{forumId:guid}/topics")]
    public async Task<IActionResult> GetTopic(
        [FromRoute] Guid forumId,
        [FromQuery] int skip,
        [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var query = new GetTopicsQuery(forumId, skip, take);

        var obj = await sender.Send(query, cancellationToken);

        return Ok(obj);
    }

    [HttpPost]
    [ProducesResponseType(403)]
    [ProducesResponseType(410)]
    [ProducesResponseType(201, Type = typeof(ForumResponse))]
    public async Task<IActionResult> CreateForum(
        [FromBody] CreateForumRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateForumCommand(request.Title);

        var forum = await sender.Send(command, cancellationToken);

        return CreatedAtRoute(nameof(GetForums), mapper.Map<ForumResponse>(forum));
    }
}