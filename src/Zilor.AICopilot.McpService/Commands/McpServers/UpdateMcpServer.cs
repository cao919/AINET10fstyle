using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.McpServer.Aggregates.McpServerInfo;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.McpService.Commands.McpServers;

[AuthorizeRequirement("Mcp.UpdateMcpServer")]
public record UpdateMcpServerCommand(
    Guid Id,
    string Name,
    string Description,
    McpTransportType TransportType,
    string? Command,
    string Arguments,
    bool IsEnabled,
    List<string>? SensitiveTools) : ICommand<Result>;

public class UpdateMcpServerCommandHandler(IRepository<McpServerInfo> repo)
    : ICommandHandler<UpdateMcpServerCommand, Result>
{
    public async Task<Result> Handle(UpdateMcpServerCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return Result.Failure("McpServer not found.");

        existing.UpdateInfo(
            request.Name,
            request.Description,
            request.TransportType,
            request.Command,
            request.Arguments,
            request.IsEnabled,
            request.SensitiveTools);

        repo.Update(existing);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
