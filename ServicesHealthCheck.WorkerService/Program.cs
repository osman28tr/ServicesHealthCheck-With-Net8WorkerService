using ServicesHealthCheck.Business;
using ServicesHealthCheck.Shared.Settings;
using ServicesHealthCheck.WorkerService;
using ServicesHealthCheck.WorkerService.BackgroundServices;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<HealthCheckBackgroundService>();

var option = builder.Configuration.GetSection("Notifications:Email");
builder.Services.Configure<MailSetting>(option);

builder.Services.AddApplicationServices();
var host = builder.Build();

host.Run();
