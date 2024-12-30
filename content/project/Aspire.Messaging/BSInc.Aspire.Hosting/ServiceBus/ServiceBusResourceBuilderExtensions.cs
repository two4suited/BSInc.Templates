namespace Aspire.Hosting.ApplicationModel;

public static class ServiceBusResourceBuilderExtensions
{
    public static IResourceBuilder<ServiceBusResource> AddServiceBus(
        this IDistributedApplicationBuilder builder,
        string name,
        string configFilePath,
        int? serviceBusPort = 5672,
        string? sqlPassword = "Pa$$word",
        int? sqlPort = 1433)
    {
        var resource = new ServiceBusResource(name);
        
        var saPassword = builder.AddParameter("password",secret:true);
        
        var sql = builder.AddSqlServer("sql", saPassword, sqlPort)
            .WithImage("azure-sql-edge")
            .WithLifetime(ContainerLifetime.Persistent);
        
        var db = sql.AddDatabase("database");
        
        return builder.AddResource(resource)
            .WithImage("mcr.microsoft.com/azure-messaging/servicebus-emulator")
            .WithReference(sql)
            .WaitFor(sql)
            .WithVolume($"{configFilePath}:/ServiceBus_Emulator/ConfigFiles/Config.json")
            .WithLifetime(ContainerLifetime.Persistent)
            .WithEndpoint(targetPort:5672, port:serviceBusPort,name: ServiceBusResource.ServiceBusEndpointName,scheme:"sb")
            .WithEnvironment("MSSQL_SA_PASSWORD", sql.Resource.PasswordParameter.Value)
            .WithEnvironment("SQL_SERVER", "sql")
            .WithEnvironment("ACCEPT_EULA", "Y");
        
    }
}

