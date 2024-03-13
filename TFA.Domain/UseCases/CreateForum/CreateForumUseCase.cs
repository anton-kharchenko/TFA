using FluentValidation;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums.Forum;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IValidator<CreateForumCommand> validator,
    IIntentionManager intentionManager,
    ICreateForumStorage storage) : ICreateForumUseCase
{
    public async Task<Forum> ExecuteAsync(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);
        return await storage.Create(command.Title, cancellationToken);
    }
}