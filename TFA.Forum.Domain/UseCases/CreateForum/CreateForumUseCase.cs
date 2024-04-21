using MediatR;
using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Commands.CreateForum;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authorization;
using TFA.Forum.Domain.Interfaces.Storages.Forum;

namespace TFA.Forum.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IIntentionManager intentionManager,
    ICreateForumStorage storage)
    : IRequestHandler<CreateForumCommand, Models.Forum>
{
    public async Task<Models.Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(ForumIntentionType.Create);
        var storageResult = await storage.CreateAsync(command.Title, cancellationToken);
        return storageResult;
    }
}