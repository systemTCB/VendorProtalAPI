using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using VendorPortal.Infrastructure.IoC;
using VendorPortal.Infrastructure.IoC.Middleware;
using Serilog;
using Serilog.Extensions;
using VendorPortal.API.Middleware;
using VendorPortal.Application.Services.v1;
using VendorPortal.Application.Services.SyncExternalData;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());


builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "VendorPortal.API", Version = "v1" });
    // Add security definition for JWT Bearer
    // Add a security definition for JWT Bearer Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Add security requirement for the endpoints
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger); // Register Serilog.ILogger
builder.Services.AddServices(builder.Configuration); 
builder.Services.AddControllersWithViews();

// builder.Services.AddHealthChecks()
//     .AddCheck<HealthCheckDisburseDataService>("hc", failureStatus: HealthStatus.Unhealthy);


builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
    options.Period = TimeSpan.FromSeconds(3600);
});
//builder.Services.AddHostedService<KubbossService>();

// builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
// builder.Services.AddHostedService<RetryReceiverService>();

builder.Services.AddResponseCompression();

var app = builder.Build();
app.UseDefaultFiles();

app.UseStaticFiles();

app.UseSwagger();
app.UseResponseCompression();
app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure the HTTP request pipeline.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VendorPortal.API v1"));
app.UseMiddleware<MiddlewareLogger>();
app.UseMiddleware<TokenVerificationMiddleware>();

app.UseRouting();
app.MapControllers();

app.Run();