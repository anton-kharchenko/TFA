using MediatR;
using TFA.Domain.Models;

namespace TFA.Domain.Commands.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title) : IRequest<Topic>;