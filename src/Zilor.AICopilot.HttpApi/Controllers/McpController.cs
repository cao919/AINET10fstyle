using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zilor.AICopilot.HttpApi.Infrastructure;
using Zilor.AICopilot.McpService.Commands.McpServers;
using Zilor.AICopilot.McpService.Queries.McpServers;

namespace Zilor.AICopilot.HttpApi.Controllers;

[Route("/api/mcp")]
[Authorize]
public class McpController : ApiControllerBase
{
    [HttpGet("server/list")]
    public async Task<IActionResult> GetListMcpServers()
    {
        var result = await Sender.Send(new GetListMcpServersQuery());
        return ReturnResult(result);
    }

    [HttpPost("server")]
    public async Task<IActionResult> CreateMcpServer(CreateMcpServerCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpPut("server")]
    public async Task<IActionResult> UpdateMcpServer(UpdateMcpServerCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpDelete("server")]
    public async Task<IActionResult> DeleteMcpServer(DeleteMcpServerCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }
}
