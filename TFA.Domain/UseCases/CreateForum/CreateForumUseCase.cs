using FluentValidation;
using MediatR;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;
using TFA.Domain.Validations.CreateForum;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IValidator<CreateForumCommandValidator> validator,
    IIntentionManager intentionManager,
    ICreateForumStorage storage,
    DomainMetrics domainMetrics) : IRequestHandler<CreateForumCommand, Forum>
{
    public async Task<Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await validator.ValidateAsync(new CreateForumCommandValidator(), cancellationToken);
            intentionManager.ThrowIfForbidden(ForumIntentionType.Create);
            var storageResult = await storage.CreateAsync(command.Title, cancellationToken);
            domainMetrics.ForumsCreated(true);
            return storageResult;
        }
        catch (Exception e)
        {
            domainMetrics.ForumsCreated(false);
            Console.WriteLine(e);
            throw;
        }
    }
}