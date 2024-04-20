using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using TFA.Domain.Models;

namespace TFA.Api;

public class KafkaConsumer(IConsumer<byte[], byte[]> consumer, ILogger<KafkaConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stopingToken)
    {
        await Task.Yield();
        
       logger.LogInformation("Subscribing to topic");
       consumer.Subscribe("tfa.DomainEvents");
       
       while (!stopingToken.IsCancellationRequested)
       {
           var consumeResult = consumer.Consume(stopingToken);
           var domainEvent = JsonSerializer.Deserialize<DomainEvent>(consumeResult.Message.Value);
           var contentBlob = Convert.FromBase64String(domainEvent!.ContentBlob);
           var topic = JsonSerializer.Deserialize<Topic>(contentBlob);
           logger.LogInformation($"Message received with id: {topic!.Id}");
           consumer.Commit(consumeResult);
       }
       
       consumer.Close();
    }
    
    public class DomainEvent
    {
        public string ContentBlob { get; set; } = null!;
    }
}