using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Klc.Mutabix.Application;
using Klc.Mutabix.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

// Clean Architecture DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers with camelCase JSON + enum converter
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Klc.Mutabix API", Version = "v1" });
});

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!))
        };
    });

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CompanyRead", policy => policy.RequireRole("Admin", "Company.Read", "Company.Write"));
    options.AddPolicy("CompanyWrite", policy => policy.RequireRole("Admin", "Company.Write"));
    options.AddPolicy("ReconciliationRead", policy => policy.RequireRole("Admin", "Reconciliation.Read", "Reconciliation.Write"));
    options.AddPolicy("ReconciliationWrite", policy => policy.RequireRole("Admin", "Reconciliation.Write"));
    options.AddPolicy("ReconciliationSend", policy => policy.RequireRole("Admin", "Reconciliation.Send"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? ["http://localhost:1700"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Create tables + seed database (runs on all environments)
await Klc.Mutabix.Infrastructure.Persistence.MutabixSeeder.SeedAsync(app.Services);

// Middleware pipeline
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Klc.Mutabix API v1"));

app.UseMiddleware<Klc.Mutabix.Api.Middleware.ExceptionMiddleware>();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health check
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithTags("Health");

app.Run();

// For integration tests
public partial class Program;
