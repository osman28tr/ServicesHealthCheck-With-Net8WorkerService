using System.Reflection;
using ServicesHealthCheck.Business;
using ServicesHealthCheck.DataAccess;
using ServicesHealthCheck.DataAccess.Concrete.NoSQL.MongoDb.Contexts;
using ServicesHealthCheck.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
