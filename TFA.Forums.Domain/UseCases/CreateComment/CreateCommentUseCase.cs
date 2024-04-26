using MediatR;
using TFA.Forums.Domain.Commands.CreateComment;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Events;
using TFA.Forums.Domain.Exceptions;
using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Domain.Interfaces.Storages.Comment;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateComment;

internal class CreateCommentUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCommentCommand, Comment>
{
    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        await using var scope = await unitOfWork.StartScopeAsync(cancellationToken);
        var storage = scope.GetStorage<ICreateCommentStorage>();
        var topic = await storage.FindTopicAsync(request.TopicId, cancellationToken);
        if (topic is null)
        {
            throw new TopicNotFoundException(request.TopicId);
        }

        intentionManager.ThrowIfForbidden(TopicIntentionType.CreateComment, topic);
        var domainEventsStorage = scope.GetStorage<IDomainEventStorage>();
        var comment = await storage.CreateAsync(request.TopicId, identityProvider.Current.UserId, request.Text, cancellationToken);
        await domainEventsStorage.AddEventAsync(ForumDomainEvent.CommentCreated(topic, comment), cancellationToken);
        
        await scope.CommitAsync(cancellationToken);
        return comment;
    }
}