using Azure.Storage.Blobs;
using Core.Configs;
using Cortex.Mediator.DependencyInjection;
using FluentValidation;
using Hangfire;
using Infastructure.Behaviour.RequestLoggingPipeline;
using Infastructure.Behaviour.RequestValidatorPipeline;
using Infastructure.Data;
using Mapster;
using MapsterMapper;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Util.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().SetPreflightMaxAge(TimeSpan.FromMinutes(10));
}));

//Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
// REGISTERED SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Fitness Api", Version = "1.0.0" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token without Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// REGISTERED MAPSTER SERVICE
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(Program).Assembly);
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<String>("AppSettings:Jwt:Issuer"),
        ValidAudience = builder.Configuration.GetValue<String>("AppSettings:Jwt:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<String>("AppSettings:Jwt:Key") ?? ""))
    };
});
// REGISTERED CORTEX MEDIATOR
builder.Services.AddCortexMediator(builder.Configuration, new[] { typeof(Program) }, options =>
{
    options.AddOpenCommandPipelineBehavior(typeof(RequestLoggingPipelineBehavior<,>));
    options.AddOpenCommandPipelineBehavior(typeof(ValidationBehavior<,>));
});
// REGISTERED ODATA
builder.Services.AddOData();
// REGISTERED FLUENT VALIDATION
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
// REGISTERED EF CORE
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
}
);



var app = builder.Build();
app.UseCors("corsapp");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();
app.UseErrorHandlingMiddleware();
app.UseSerilogRequestLogging();
app.UseEndpoints(endpoints =>
{
    endpoints.EnableDependencyInjection();

});
app.MapControllers();
app.Run();
