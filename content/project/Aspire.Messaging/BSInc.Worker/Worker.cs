using Azure.Messaging.ServiceBus;

namespace BSInc.Worker;

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
        
        var sender = _client.CreateSender("queue.1");

        using var messageBatch = await sender.CreateMessageBatchAsync();
        
        for (var i = 0; i <= numberOfMessages; i++)
        {
            if(!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}"))) continue;
            {
                _logger.LogError("Error");
                throw new Exception($"The message {i} is too large to fit in the batch.");
            }
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
            Console.WriteLine("A batch of {numberOfMessages} messages has been sent.");
            _logger.LogInformation("A batch of {numberOfMessages} messages has been sent.", numberOfMessages);
        }
        finally
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            await sender.DisposeAsync();
        }
        
       
    }
}