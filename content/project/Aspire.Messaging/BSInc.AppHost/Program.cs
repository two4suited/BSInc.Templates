var builder = DistributedApplication.CreateBuilder(args);

var saPassword = builder.AddParameter("password",secret:true);
var port = 1433;

var sql = builder.AddSqlServer("sql", saPassword,port)
    .WithImage("azure-sql-edge")
    .WithLifetime(ContainerLifetime.Persistent);


var db = sql.AddDatabase("database");

var servicebus = builder.AddContainer("servicebus", "mcr.microsoft.com/azure-messaging/servicebus-emulator")
    .WithReference(sql)
    .WithEnvironment("MSSQL_SA_PASSWORD", sql.Resource.PasswordParameter.Value)
    .WithEnvironment("SQL_SERVER", "sql")
    .WithEnvironment("ACCEPT_EULA", "Y");
   


builder.Build().Run();
