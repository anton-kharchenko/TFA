using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.CreateForum;

public interface ICreateForumUseCase
{
    Task<Forum> ExecuteAsync(CreateForumCommand command, CancellationToken cancellationToken);
}