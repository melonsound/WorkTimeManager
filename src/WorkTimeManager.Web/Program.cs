using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkTimeManager.Core.Interfaces;
using WorkTimeManager.Core.Models;
using WorkTimeManager.Infrastructure;
using WorkTimeManager.Infrastructure.Data;
using WorkTimeManager.Infrastructure.Interfaces;
using WorkTimeManager.Infrastructure.Services;
using WorkTimeManager.Infrastructure.Validators;
using WorkTimeManager.Security;
using WorkTimeManager.Security.Data;
using WorkTimeManager.Security.Models;
using WorkTimeManager.Security.Services;
using WorkTimeManager.Security.Validatiors;

var builder = WebApplication.CreateBuilder(args);

var connString = Environment.GetEnvironmentVariable("MAIN_DB"); 
var securityConnString = Environment.GetEnvironmentVariable("SECURITY_DB");
var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("APP_JWT_ISSUERKEY"));

// #Startup
#region Services

builder.Services.AddControllers();
builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddSwaggerGen();
builder.Services.AddSecurityDbContext(securityConnString);
builder.Services.AddDbContext(connString);
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IValidator<AppUser>, AppUserValidator>();
builder.Services.AddScoped<IValidator<TaskEntity>, TaskEntityValidator>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IGoogleDriveCloudService, GoogleDriveCloudService>(x => 
    new GoogleDriveCloudService("GoogleDrive/client_secret_desktop.json", "worktimemanager", Environment.GetEnvironmentVariable("APP_GOOGLEDRIVE_FOLDERID")));

builder.Services.AddIdentity<AppUser, AppUserRole>(options =>
{
    builder.Configuration.GetSection("IdentityOptions").Bind(options);
})
.AddEntityFrameworkStores<AppSecurityDbContext>()
.AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false
        
    };
});

#if !DEBUG
//
#endif
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

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
