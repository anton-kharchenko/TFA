using MediatR;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.Commands.CreateComment;

public class CreateCommentCommand : IRequest<Comment>
{
    public CreateCommentCommand(Guid topicId, string text)
    {
        TopicId = topicId;
        Text = text;
    }

    public Guid TopicId { get; set; }
    
    public string Text { get; set; }
}