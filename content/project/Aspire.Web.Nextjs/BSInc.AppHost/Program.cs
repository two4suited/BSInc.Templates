var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                      .WithPgAdmin();

var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.BSInc_API>("apiservice");

var nextjs = builder.AddNpmApp("nextjs","../BSInc.Web")
                    .WithReference(apiService)
                    .WithEnvironment("BROWSER", "none")
                    .WithHttpEndpoint(env: "PORT")
                    .WithExternalHttpEndpoints()
                    .PublishAsDockerFile();



/*
builder.AddProject<Projects.AspireTemplate_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);
*/
builder.Build().Run();
