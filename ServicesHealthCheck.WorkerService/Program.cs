using ServicesHealthCheck.Business;
using ServicesHealthCheck.DataAccess;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Contexts;
using ServicesHealthCheck.Shared.Settings;
using ServicesHealthCheck.WorkerService;
using ServicesHealthCheck.WorkerService.BackgroundServices;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<HealthCheckBackgroundService>();

var option = builder.Configuration.GetSection("Notifications:Email");
builder.Services.Configure<MailSetting>(option);
IConfiguration configuration = builder.Configuration;
builder.Services.AddSingleton<HealthCheckContext>();
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices();
var host = builder.Build();

host.Run();
