using MediatR;
using TFA.Forum.Domain.Models;

namespace TFA.Forum.Domain.Commands.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<Topic>;