using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.McpServer.Aggregates.McpServerInfo;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.McpService.Queries.McpServers;

public record McpServerDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string TransportType { get; set; }
    public string? Command { get; set; }
    public required string Arguments { get; set; }
    public bool IsEnabled { get; set; }
    public List<string>? SensitiveTools { get; set; }
}

[AuthorizeRequirement("Mcp.GetListMcpServers")]
public record GetListMcpServersQuery : IQuery<Result<IList<McpServerDto>>>;

public class GetListMcpServersQueryHandler(
    IDataQueryService dataQueryService)
    : IQueryHandler<GetListMcpServersQuery, Result<IList<McpServerDto>>>
{
    public async Task<Result<IList<McpServerDto>>> Handle(GetListMcpServersQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = dataQueryService.McpServerInfos
            .Select(s => new McpServerDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                TransportType = s.TransportType.ToString(),
                Command = s.Command,
                Arguments = s.Arguments,
                IsEnabled = s.IsEnabled,
                SensitiveTools = s.SensitiveTools
            });
        var result = await dataQueryService.ToListAsync(queryable);
        return Result.Success(result);
    }
}
