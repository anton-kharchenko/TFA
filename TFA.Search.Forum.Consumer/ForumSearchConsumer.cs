using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TFA.Search.API.Grpc;
using TFA.Search.Forum.Consumer.Enums;
using TFA.Search.Forum.Consumer.Events;
using TFA.Search.Forum.Consumer.Models;

namespace TFA.Search.Forum.Consumer;

internal class ForumSearchConsumer(
    IConsumer<byte[], byte[]> consumer,
    SearchEngine.SearchEngineClient searchEngineClient,
    IOptions<ConsumerConfig> consumerConfig) : BackgroundService
{
    private static readonly ActivitySource ActivitySource = new ("ForumSearchConsumer");
    private readonly ConsumerConfig _consumerConfig = consumerConfig.Value;
    
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

            var activityId = consumeResult.Message.Headers.TryGetLastBytes("activity_id", out var lastBytes)
                ? Encoding.UTF8.GetString(lastBytes)
                : null;
            
            using var activity = ActivitySource.StartActivity("ForumSearchConsumer.Kafka.Consume", 
                ActivityKind.Consumer, 
                ActivityContext.TryParse(activityId, null, out var context) ? context : default);
            
            activity?.AddTag("messaging.system", "kafka");
            activity?.AddTag("messaging.destination.name", "tfa.DomainEvents");
            activity?.AddTag("messaging.kafka.consumer_group", _consumerConfig.GroupId);
            activity?.AddTag("messaging.kafka.partition", consumeResult.Partition);
            
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