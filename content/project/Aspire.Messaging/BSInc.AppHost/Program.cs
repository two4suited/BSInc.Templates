var builder = DistributedApplication.CreateBuilder(args);

var saPassword = builder.AddParameter("password",secret:true);
var port = 1433;
var serviebusConfigfilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");;

var sql = builder.AddSqlServer("sql", saPassword, port)
    .WithImage("azure-sql-edge");
   


var db = sql.AddDatabase("database");

var servicebus = builder.AddContainer("servicebus", "mcr.microsoft.com/azure-messaging/servicebus-emulator")
    .WithReference(sql)
    .WaitFor(sql)
    .WithVolume($"{serviebusConfigfilePath}:/ServiceBus_Emulator/ConfigFiles/Config.json")
    .WithEnvironment("MSSQL_SA_PASSWORD", sql.Resource.PasswordParameter.Value)
    .WithEnvironment("SQL_SERVER", "sql")
    .WithEnvironment("ACCEPT_EULA", "Y");
   


builder.Build().Run();
