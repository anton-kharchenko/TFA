using System.Text.Json;
using Confluent.Kafka;
using TFA.Search.API.Grpc;

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

            var domainEvent = JsonSerializer.Deserialize<DomainEvent>(consumeResult.Message.Value);
            var contentBlob = Convert.FromBase64String(domainEvent!.ContentBlob);
            var topic = JsonSerializer.Deserialize<Topic>(contentBlob);

           await searchEngineClient.IndexAsync(new IndexRequest()
            {
                Id = topic!.Id.ToString(),
                Type = SearchEntityType.ForumTopic,
                Title = topic.Title
            }, cancellationToken:stoppingToken);
            
            consumer.Commit(consumeResult);
        }
        
        consumer.Close();
    }
}

internal class Topic
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = default!;
}

internal class DomainEvent
{
    public string ContentBlob { get; set; } = default!;
}