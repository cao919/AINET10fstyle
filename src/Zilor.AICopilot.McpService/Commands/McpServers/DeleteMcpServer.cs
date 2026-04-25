using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.McpServer.Aggregates.McpServerInfo;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.McpService.Commands.McpServers;

[AuthorizeRequirement("Mcp.DeleteMcpServer")]
public record DeleteMcpServerCommand(Guid Id) : ICommand<Result>;

public class DeleteMcpServerCommandHandler(IRepository<McpServerInfo> repo)
    : ICommandHandler<DeleteMcpServerCommand, Result>
{
    public async Task<Result> Handle(DeleteMcpServerCommand request, CancellationToken cancellationToken)
    {
        var server = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (server == null) return Result.Success();

        repo.Delete(server);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
