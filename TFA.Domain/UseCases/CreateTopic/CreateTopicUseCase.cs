﻿using FluentValidation;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Exceptions;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.UseCases.CreateTopic;

namespace TFA.Domain.UseCases.CreateTopic;

internal class CreateTopicUseCase(
 IIntentionManager intentionManager,
 ICreateTopicStorage storage, 
 IIdentityProvider identityProvider, 
 IValidator<CreateTopicCommand> validator) : ICreateTopicUseCase
{
    public async Task<Models.Topic> ExecuteAsync(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command ,cancellationToken);
        
        var (forumId, title) = (command.ForumId, command.Title);
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        var forumExists = await storage.ForumExistsAsync(forumId, cancellationToken);
        
        if (forumExists is false) throw new ForumNotFoundException(forumId);
        
        return await storage.CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}