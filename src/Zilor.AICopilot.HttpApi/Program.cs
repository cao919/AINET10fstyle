using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Zilor.AICopilot.HttpApi;
using Zilor.AICopilot.Infrastructure;
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;
var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? nameof(Zilor.AICopilot.HttpApi);
var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://localhost:4317";
            
var resource = ResourceBuilder.CreateDefault()
    .AddService(serviceName);
            
Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resource)
    .AddSource(nameof(Zilor.AICopilot.AiGatewayService))
    .AddSource("*Microsoft.Agents.AI*")
    .AddSource("*Microsoft.Extensions.AI*")
    .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint))
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddInfrastructures();
builder.AddApplicationService();
builder.AddWebServices();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Zilor AI Copilot API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "请输入 JWT Token（不需要加 Bearer 前缀）"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zilor AI Copilot API v1");
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
