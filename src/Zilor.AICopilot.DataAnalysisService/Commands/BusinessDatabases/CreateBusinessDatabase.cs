using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.DataAnalysis.Aggregates.BusinessDatabase;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.DataAnalysisService.Commands.BusinessDatabases;

public record CreatedBusinessDatabaseDto(Guid Id, string Name);

[AuthorizeRequirement("DataAnalysis.CreateBusinessDatabase")]
public record CreateBusinessDatabaseCommand(
    string Name,
    string Description,
    string ConnectionString,
    DbProviderType Provider) : ICommand<Result<CreatedBusinessDatabaseDto>>;

public class CreateBusinessDatabaseCommandHandler(IRepository<BusinessDatabase> repo)
    : ICommandHandler<CreateBusinessDatabaseCommand, Result<CreatedBusinessDatabaseDto>>
{
    public async Task<Result<CreatedBusinessDatabaseDto>> Handle(CreateBusinessDatabaseCommand request,
        CancellationToken cancellationToken)
    {
        var db = new BusinessDatabase(
            request.Name,
            request.Description,
            request.ConnectionString,
            request.Provider);

        repo.Add(db);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreatedBusinessDatabaseDto(db.Id, db.Name));
    }
}
