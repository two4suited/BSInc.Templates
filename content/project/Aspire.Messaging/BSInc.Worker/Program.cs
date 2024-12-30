using Azure.Messaging.ServiceBus;
using BSInc.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ServiceBusClient>(sbClient =>
{

    var clientOptions = new ServiceBusClientOptions()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    };
    return new ServiceBusClient(builder.Configuration.GetConnectionString("servicebus"));
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();