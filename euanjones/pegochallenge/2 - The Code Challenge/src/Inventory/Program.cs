using Inventory;
using Inventory.Application;
using Inventory.Books;
using Inventory.Core;
using Inventory.Inventory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.Configure<CacheOptions>(configuration.GetRequiredSection(CacheOptions.SectionName));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOutputCache(options =>
{
	// Would use a distributed cache with horizontally scaling
	options.SizeLimit = 512;
	options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(15);
});
builder.Services.AddMediatR(m =>
{
	m.RegisterServicesFromAssemblyContaining<IAssemblyMarker>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(configuration);
builder.Services.AddGrpc();
builder.Services.AddGrpcHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseExceptionHandler();
app.UseOutputCache();

app.CreateBookEndpoints();
app.MapGrpcHealthChecksService();
app.MapGrpcService<InventoryServiceImpl>();

app.MapGet("/", static () => $"Inventory Service version {Assembly.GetEntryAssembly()?.GetName().Version}");

app.Run();

// Enable integration testing
public partial class Program;