using System.Reflection;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using ServicesHealthCheck.Business;
using ServicesHealthCheck.DataAccess;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Contexts;
using ServicesHealthCheck.Monitoring.BackgroundServices;
using ServicesHealthCheck.Monitoring.SignalRHubs;
using ServicesHealthCheck.Shared;
using ServicesHealthCheck.Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<HealthCheckBackgroundService>();
builder.Services.AddHostedService<HealthCheckByTimeBackgroundService>();
//builder.Services.AddHostedService<ServiceEventViewerLogBackgroundService>();
builder.Services.AddWindowsService();

builder.Services.AddSignalR();

// Log configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
// Add Serilog Library
builder.Logging.AddSerilog(logger);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5181);
});

var option = builder.Configuration.GetSection("Notifications:Email");
builder.Services.Configure<MailSetting>(option);

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    string date = DateTime.Now.ToString("yyyyMMdd");
    configuration
        .MinimumLevel.Information().
    WriteTo.File($"Logs/minlevelinfo{date}log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.File($"Logs/minlevelwarning{date}log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File($"Logs/minlevelerror{date}log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
    .WriteTo.File($"Logs/minlevelfatal{date}log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal);
});

builder.Services.AddSingleton<HealthCheckContext>();
builder.Services.AddSharedServices();
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.MapHub<HealthCheckHub>("/healthcheckhub");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HealthCheck}/{action=Index}/{id?}");

app.Run();
