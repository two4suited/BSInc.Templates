var builder = DistributedApplication.CreateBuilder(args);

var serviebusConfigfilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

var sb = builder.AddServiceBus("servicebus",serviebusConfigfilePath);

var worker = builder.AddProject<Projects.BSInc_Publish>("publisher")
    .WithReference(sb)
    .WaitFor(sb);

builder.Build().Run();
