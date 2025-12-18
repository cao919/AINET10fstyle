using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Zilor.AICopilot.HttpApi;
using Zilor.AICopilot.Infrastructure;

// 1. 获取服务名称，这决定了在仪表板 里看到的名字
var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? nameof(Zilor.AICopilot.HttpApi);
// 2. 获取数据上报地址，Aspire 会自动注入这个环境变量
var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://localhost:4317";

var resource = ResourceBuilder.CreateDefault()
    .AddService(serviceName);

// 3. 构建追踪提供程序
Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resource)
    // 核心配置：监听 Agent 框架发出的信号
    .AddSource(nameof(Zilor.AICopilot.AiGatewayService))     // 监听我们自己业务代码的 Activity
    .AddSource("*Microsoft.Agents.AI")       // 监听 Agent Framework 的核心事件
    .AddSource("*Microsoft.Extensions.AI*")   // 监听底层 AI 扩展库的事件
    .AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint)) // 将数据导出到 Aspire Dashboard
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddInfrastructures();
builder.AddApplicationService();
builder.AddWebServices();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();