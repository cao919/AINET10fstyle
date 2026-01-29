using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using Zilor.AICopilot.AgentPlugin;
using Zilor.AICopilot.Core.McpServer.Aggregates.McpServerInfo;
using Zilor.AICopilot.Services.Common.Contracts;

namespace Zilor.AICopilot.McpService;

public class McpServerBootstrap(
    IDataQueryService dataQueryService,
    AgentPluginLoader agentPluginLoader,
    ILogger<McpServerBootstrap> logger)
    : IMcpServerBootstrap
{
    public async IAsyncEnumerable<McpClient> StartAsync([EnumeratorCancellation] CancellationToken ct)
    {
        var query = dataQueryService.McpServerInfos
            .Where(m => m.IsEnabled);

        var mcpServerInfos = await dataQueryService.ToListAsync(query);

        foreach (var mcpServerInfo in mcpServerInfos)
        {
            McpClient mcpClient = null!;
            switch (mcpServerInfo.TransportType)
            {
                case McpTransportType.Stdio:
                    mcpClient = await CreateStdioClientAsync(mcpServerInfo, ct);
                    break;
                case McpTransportType.Sse:
                    mcpClient = await CreateSseClientAsync(mcpServerInfo, ct);
                    break;
            }

            logger.LogInformation(
                "已连接到 MCP 服务器 - {Name}",
                mcpServerInfo.Name);

            var tools = await mcpClient.ListToolsAsync(
                cancellationToken: ct);

            logger.LogInformation(
                "已发现 {ToolsCount} 个工具",
                tools.Count);
            
            // 2. 构建并注册适配器插件
            // 这一步将 MCP 的数据模型转换为 Agent 的插件模型
            RegisterMcpPlugin(mcpServerInfo, tools);
            
            logger.LogInformation(
                "已注册 MCP 插件 - {Name}",
                mcpServerInfo.Name);
            
            yield return mcpClient;
        }
    }
    
    /// <summary>
    /// 将 MCP 服务元数据和工具列表封装为通用桥接插件，并注册到系统。
    /// </summary>
    /// <param name="info">数据库中的服务配置信息</param>
    /// <param name="mcpTools">从 MCP Client 获取的实时工具列表</param>
    private void RegisterMcpPlugin(McpServerInfo info, IEnumerable<AITool> mcpTools)
    {
        var mcpPlugin = new GenericBridgePlugin
        {
            // 名称作为命名空间，至关重要
            Name = info.Name, 
            
            // 描述用于语义路由
            Description = info.Description, 
            
            // 直接传递工具集合
            AITools = mcpTools 
        };

        // 注册到全局插件系统
        agentPluginLoader.RegisterAgentPlugin(mcpPlugin);
    }

    private async Task<McpClient> CreateStdioClientAsync(McpServerInfo mcpServerInfo, CancellationToken ct)
    {
        var transportOptions = new StdioClientTransportOptions
        {
            Command = "npx",
            Arguments = mcpServerInfo.Arguments.Split(' ')
        };

        var transport = new StdioClientTransport(transportOptions);
        return await McpClient.CreateAsync(
            transport,
            cancellationToken: ct);
    }
    
    private async Task<McpClient> CreateSseClientAsync(McpServerInfo mcpServerInfo, CancellationToken ct)
    {
        var transportOptions = new HttpClientTransportOptions
        {
            Endpoint = new Uri(mcpServerInfo.Arguments)
        };

        var transport = new HttpClientTransport(transportOptions);
        return await McpClient.CreateAsync(
            transport,
            cancellationToken: ct);
    }
}