using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

public class CreateForumUseCase : ICreateForumUseCase
{
    public async Task<Forum> ExecuteAsync(CreateForumCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}