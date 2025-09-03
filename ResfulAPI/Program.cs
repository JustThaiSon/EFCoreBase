using ResfulAPI.Extensions;
using Microsoft.OpenApi.Models;
using ResfulAPI.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var builderLog = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())  // location of the exe file
                    .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true);
IConfigurationRoot configuration = builderLog.Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var _services = builder.Services;
var _configuration = builder.Configuration;

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowOrigins";
_services.AddCoreExtention();
_services.AddDatabase(_configuration);
_services.AddServiceContext(_configuration);
_services.AddCORS(MyAllowSpecificOrigins);
_services.AddCoreService();
_services.AddMemoryCache();

_services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .ToDictionary(
                ms => ms.Key,
                ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        throw new ValidationException(
            errors.SelectMany(kv => kv.Value.Select(error => new ValidationFailure(kv.Key, error)))
        );
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
_services.AddEndpointsApiExplorer();
_services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "My Project",
        Description = "My Project api documents"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});
var app = builder.Build();
//DbInitializer.ConfigureCoreDb(app);
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run();
