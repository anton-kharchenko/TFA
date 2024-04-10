using MediatR;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IIntentionManager intentionManager,
    ICreateForumStorage storage)
    : IRequestHandler<CreateForumCommand, Forum>
{
    public async Task<Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(ForumIntentionType.Create);
        var storageResult = await storage.CreateAsync(command.Title, cancellationToken);
        return storageResult;
    }
}