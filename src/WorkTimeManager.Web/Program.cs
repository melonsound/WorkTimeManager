using Microsoft.EntityFrameworkCore;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Infrastructure;
using WorkTimeManager.Infrastructure.Data;
using WorkTimeManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connString = configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext(connString);

builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

using(var serviceScope = app.Services.CreateScope())
{
    var appDataDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    appDataDbContext.Database.Migrate();
}

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
