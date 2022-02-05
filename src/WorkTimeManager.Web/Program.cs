using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Infrastructure;
using WorkTimeManager.Infrastructure.Data;
using WorkTimeManager.Infrastructure.Services;
using WorkTimeManager.Security;
using WorkTimeManager.Security.Data;
using WorkTimeManager.Security.Models;
using WorkTimeManager.Security.Validatiors;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration["MainApp:LocalDb"];
var securityConnString = builder.Configuration["MainApp:LocalDbSecurity"];

// #Startup
#region Services

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddSecurityDbContext(securityConnString);
builder.Services.AddDbContext(connString);
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IValidator<AppUser>, AppUserValidator>();

builder.Services.AddIdentity<AppUser, AppUserRole>(options =>
{
    builder.Configuration.GetSection("IdentityOptions").Bind(options);
})
.AddEntityFrameworkStores<AppSecurityDbContext>()
.AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

#endregion


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var appDataDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    var appSecurityDataDbContext = serviceScope.ServiceProvider.GetRequiredService<AppSecurityDbContext>();
    appDataDbContext.Database.Migrate();
    appSecurityDataDbContext.Database.Migrate();
}

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
