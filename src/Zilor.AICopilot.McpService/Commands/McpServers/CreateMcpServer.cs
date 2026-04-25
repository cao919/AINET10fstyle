using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.McpServer.Aggregates.McpServerInfo;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.McpService.Commands.McpServers;

public record CreatedMcpServerDto(Guid Id, string Name);

[AuthorizeRequirement("Mcp.CreateMcpServer")]
public record CreateMcpServerCommand(
    string Name,
    string Description,
    McpTransportType TransportType,
    string? Command,
    string Arguments,
    List<string>? SensitiveTools) : ICommand<Result<CreatedMcpServerDto>>;

public class CreateMcpServerCommandHandler(IRepository<McpServerInfo> repo)
    : ICommandHandler<CreateMcpServerCommand, Result<CreatedMcpServerDto>>
{
    public async Task<Result<CreatedMcpServerDto>> Handle(CreateMcpServerCommand request,
        CancellationToken cancellationToken)
    {
        var server = new McpServerInfo(
            request.Name,
            request.Description,
            request.TransportType,
            request.Command,
            request.Arguments,
            request.SensitiveTools);

        repo.Add(server);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreatedMcpServerDto(server.Id, server.Name));
    }
}
