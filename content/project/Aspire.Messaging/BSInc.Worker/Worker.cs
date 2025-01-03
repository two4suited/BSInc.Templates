using Azure.Messaging.ServiceBus;

namespace BSInc.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int numOfMessagesPerBatch = 10;
        const int numOfBatches = 10;

        string queueName = "queue.1";

        var client = new ServiceBusClient(_connectionString);
        var sender = client.CreateSender(queueName);

        for (int i = 1; i <= numOfBatches; i++)
        {
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int j = 1; j <= numOfMessagesPerBatch; j++)
            {
                messageBatch.TryAddMessage(new ServiceBusMessage($"Batch:{i}:Message:{j}"));
            }
            await sender.SendMessagesAsync(messageBatch);
        }

        await sender.DisposeAsync();
        await client.DisposeAsync();

        Console.WriteLine($"{numOfBatches} batches with {numOfMessagesPerBatch} messages per batch has been published to the queue.");
        
       
    }
}