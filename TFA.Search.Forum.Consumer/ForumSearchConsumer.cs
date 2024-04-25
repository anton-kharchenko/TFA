using System.Text.Json;
using Confluent.Kafka;
using TFA.Search.API.Grpc;
using TFA.Search.Forum.Consumer.Enums;
using TFA.Search.Forum.Consumer.Events;
using TFA.Search.Forum.Consumer.Models;

namespace TFA.Search.Forum.Consumer;

internal class ForumSearchConsumer(
    IConsumer<byte[], byte[]> consumer,
    SearchEngine.SearchEngineClient searchEngineClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        consumer.Subscribe("tfa.DomainEvents");
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            
            if (consumeResult is not { IsPartitionEOF: false })
            {
                await Task.Delay(300, stoppingToken);
                continue;
            }

            var domainEvent = JsonSerializer.Deserialize<DomainEventWrapper>(consumeResult.Message.Value);
            var contentBlob = Convert.FromBase64String(domainEvent!.ContentBlob);
            var forumDomainEvent = JsonSerializer.Deserialize<ForumDomainEvent>(contentBlob);

            switch (forumDomainEvent!.EventType)
            {
                case ForumDomainEventType.TopicCreated:
                    await searchEngineClient.IndexAsync(new IndexRequest()
                    {
                        Id = forumDomainEvent!.TopicId.ToString(),
                        Type = SearchEntityType.ForumTopic,
                        Title = forumDomainEvent.Title
                    }, cancellationToken:stoppingToken);
                    break;
                case ForumDomainEventType.TopicUpdated:
                    break;
                case ForumDomainEventType.TopicDeleted:
                    break;
                case ForumDomainEventType.CommentCreated:
                    break;
                case ForumDomainEventType.CommentUpdated:
                    break;
                case ForumDomainEventType.CommentDeleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            consumer.Commit(consumeResult);
        }
        
        consumer.Close();
    }
}