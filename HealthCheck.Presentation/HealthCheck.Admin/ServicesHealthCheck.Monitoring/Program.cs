using System.Reflection;
using Microsoft.Extensions.Hosting.WindowsServices;
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
builder.Services.AddWindowsService();
builder.Services.AddSignalR();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5181);
});

var option = builder.Configuration.GetSection("Notifications:Email");
builder.Services.Configure<MailSetting>(option);

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
