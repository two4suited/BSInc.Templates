using Azure.Messaging.ServiceBus;
using BSInc.Publish;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSingleton<ServiceBusClient>(sbClient => new ServiceBusClient(builder.Configuration.GetConnectionString("servicebus")));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();