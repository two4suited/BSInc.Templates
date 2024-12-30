using Azure.Messaging.ServiceBus;

namespace BSInc.Publish;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ServiceBusClient _client;


    public Worker(ILogger<Worker> logger, ServiceBusClient client)
    {
        _logger = logger;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int numberOfMessages = 3;

        const int numOfMessagesPerBatch = 10;
        const int numOfBatches = 10;

        string queueName = "queue.1";
       
        var sender = _client.CreateSender(queueName);

        for (int i = 1; i <= numOfBatches; i++)
        {
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync(stoppingToken);

            for (int j = 1; j <= numOfMessagesPerBatch; j++)
            {
                messageBatch.TryAddMessage(new ServiceBusMessage($"Batch:{i}:Message:{j}"));
            }
            await sender.SendMessagesAsync(messageBatch);
        }

        await sender.DisposeAsync();
        await _client.DisposeAsync();

        Console.WriteLine($"{numOfBatches} batches with {numOfMessagesPerBatch} messages per batch has been published to the queue.");
       
    }
}