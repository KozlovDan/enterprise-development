using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("mysql-password", secret: true);
var mysql = builder.AddMySql("mysql", password).WithImageTag("8.4");
if (!builder.Configuration.GetValue<bool>("Testing"))
{
    mysql.WithDataVolume("autoservice-mysql-data");
}
var database = mysql.AddDatabase("autoservice");

var migrations = builder.AddProject<Projects.AutoService_Migrations>("migrations")
    .WithReference(database).WaitFor(database);

builder.AddProject<Projects.AutoService_Api>("api")
    .WithReference(database)
    .WithHttpHealthCheck("/health")
    .WaitForCompletion(migrations);

builder.Build().Run();
