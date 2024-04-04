using FluentValidation;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Interfaces.UseCases.CreateForum;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;
using TFA.Domain.Validations.CreateForum;

namespace TFA.Domain.UseCases.CreateForum;

internal class CreateForumUseCase(
    IValidator<CreateForumCommandValidator> validator,
    IIntentionManager intentionManager,
    ICreateForumStorage storage,
    DomainMetrics domainMetrics) : ICreateForumUseCase
{
    public async Task<Forum> ExecuteAsync(CreateForumCommand command, CancellationToken cancellationToken)
    {
        try
        {
    
            await validator.ValidateAsync(new CreateForumCommandValidator(), cancellationToken);
            intentionManager.ThrowIfForbidden(ForumIntentionType.Create);
            var storageResult = await storage.CreateAsync(command.Title, cancellationToken);
            domainMetrics.ForumsCreated();
            return storageResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}