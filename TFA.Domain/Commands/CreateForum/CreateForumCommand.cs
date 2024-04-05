using MediatR;
using TFA.Domain.Models;

namespace TFA.Domain.Commands.CreateForum;

public record CreateForumCommand(string Title) : IRequest<Forum>;