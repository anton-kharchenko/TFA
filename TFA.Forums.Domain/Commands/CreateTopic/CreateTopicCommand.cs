using MediatR;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.Commands.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<Topic>;