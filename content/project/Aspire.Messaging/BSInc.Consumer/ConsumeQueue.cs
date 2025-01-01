using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BSInc.Consumer;

public class ConsumeQueue
{
    private readonly ILogger<ConsumeQueue> _logger;

    public ConsumeQueue(ILogger<ConsumeQueue> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ConsumeQueue))]
    public async Task Run(
        [ServiceBusTrigger("queue.1", Connection = "servicebus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        Console.WriteLine($"Message ID: {message.MessageId}");
        // Complete the message
        await messageActions.CompleteMessageAsync(message);
        
    }
}