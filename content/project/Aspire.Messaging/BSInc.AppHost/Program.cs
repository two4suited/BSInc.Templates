using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var serviebusConfigfilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var sb = builder.AddServiceBus("servicebus",serviebusConfigfilePath);

var worker = builder.AddProject<Projects.BSInc_Publish>("publisher")
    .WithReference(sb)
    .WaitFor(sb);

var consumer = builder.AddAzureFunctionsProject<Projects.BSInc_Consumer>("consumer")
    .WithReference(sb)
    .WithHostStorage(storage)
    .WaitFor(storage);

builder.Build().Run();
