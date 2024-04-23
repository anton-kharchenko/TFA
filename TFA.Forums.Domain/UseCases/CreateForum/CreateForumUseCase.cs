using MediatR;
using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Commands.CreateForum;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Interfaces.Storages.Forum;

namespace TFA.Forums.Domain.UseCases.CreateForum;

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