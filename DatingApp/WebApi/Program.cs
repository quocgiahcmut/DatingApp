using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Middleware;
using WebApi.Services.Token;
using WebApi.SignalR;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(config);
builder.Services.AddIdentityServices(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowCredentials()
	.WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
	var context = services.GetRequiredService<ApplicationDbContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();
	var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
	await context.Database.MigrateAsync();
	await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
	//context.Connections.RemoveRange(context.Connections);
	await SeedData.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
	var logger = services.GetService<ILogger<Program>>();
	logger.LogError(ex, "An error occurred during migration");
}

app.Run();
