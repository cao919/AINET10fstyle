using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.DataAnalysis.Aggregates.BusinessDatabase;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.DataAnalysisService.Queries.BusinessDatabases;

public record BusinessDatabaseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ConnectionString { get; set; }
    public required string Provider { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
}

[AuthorizeRequirement("DataAnalysis.GetListBusinessDatabases")]
public record GetListBusinessDatabasesQuery : IQuery<Result<IList<BusinessDatabaseDto>>>;

public class GetListBusinessDatabasesQueryHandler(
    IDataQueryService dataQueryService)
    : IQueryHandler<GetListBusinessDatabasesQuery, Result<IList<BusinessDatabaseDto>>>
{
    public async Task<Result<IList<BusinessDatabaseDto>>> Handle(GetListBusinessDatabasesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = dataQueryService.BusinessDatabases
            .Select(db => new BusinessDatabaseDto
            {
                Id = db.Id,
                Name = db.Name,
                Description = db.Description,
                ConnectionString = db.ConnectionString,
                Provider = db.Provider.ToString(),
                IsEnabled = db.IsEnabled,
                CreatedAt = db.CreatedAt
            });
        var result = await dataQueryService.ToListAsync(queryable);
        return Result.Success(result);
    }
}
