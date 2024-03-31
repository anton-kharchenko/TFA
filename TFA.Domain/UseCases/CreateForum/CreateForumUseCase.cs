using FluentValidation;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Models;
using TFA.Domain.Validations.CreateForum;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IValidator<CreateForumCommandValidator> validator,
    IIntentionManager intentionManager,
    ICreateForumStorage storage) : ICreateForumUseCase
{
    public async Task<Forum> ExecuteAsync(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(new CreateForumCommandValidator(), cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);
        return await storage.CreateAsync(command.Title, cancellationToken);
    }
}