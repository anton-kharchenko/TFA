using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Forums.Api.Requests.Comment;
using TFA.Forums.Api.Responses;
using TFA.Forums.Domain.Commands.CreateComment;

namespace TFA.Forums.Api.Controllers;

[ApiController]
[Route("topics")]
public class TopicController(ISender sender, IMapper mapper)  : ControllerBase
{
    [HttpPost("{topicId:guid}/comments")]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CommentResponse))]
    public async Task<IActionResult> CreateComment(Guid topicId, [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(topicId, request.Text);
        var comment = await sender.Send(command, cancellationToken);
        return Ok(mapper.Map<CommentResponse>(comment));
    }
}