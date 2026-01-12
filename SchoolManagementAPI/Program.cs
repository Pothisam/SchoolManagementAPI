using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.ConfigurationModels;
using Repository;
using SchoolManagementAPI.Middleware;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configbuilder = builder.Configuration;

// Configure logging to ensure it works in production
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog(); // For Windows Event Log in production

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.Configure<AppKeyConfig>(configbuilder.GetSection("AppKeyConfig"));
builder.Services.Configure<JwtConfig>(configbuilder.GetSection("JWTConfig"));
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = configbuilder["JWTConfig:Issuer"],
        ValidAudience = configbuilder["JWTConfig:Audince"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey.AuthKey))
    };
});
// Register Services including Repository services and DbContext
builder.Services.ServicesDependencyInjection(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapse controllers by default
    });
}
else
{
    // Add global exception handler for production
    app.UseExceptionHandler("/error");
}

// Add error logging middleware early in the pipeline to capture unhandled exceptions
app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseCors(option =>
{
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.AllowAnyOrigin();
});
app.UseAuthorization();

// Global error handling endpoint
app.Map("/error", (HttpContext context) =>
{
    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionFeature?.Error;
    
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogError(exception, "Unhandled exception occurred");
    
    return Results.Problem(
        title: "An error occurred",
        statusCode: StatusCodes.Status500InternalServerError
    );
});

app.MapControllers();

app.Run();
