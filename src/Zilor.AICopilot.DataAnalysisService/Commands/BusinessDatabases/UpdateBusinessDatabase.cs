using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.DataAnalysis.Aggregates.BusinessDatabase;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.DataAnalysisService.Commands.BusinessDatabases;

[AuthorizeRequirement("DataAnalysis.UpdateBusinessDatabase")]
public record UpdateBusinessDatabaseCommand(
    Guid Id,
    string Name,
    string Description,
    string ConnectionString,
    DbProviderType Provider,
    bool IsEnabled) : ICommand<Result>;

public class UpdateBusinessDatabaseCommandHandler(IRepository<BusinessDatabase> repo)
    : ICommandHandler<UpdateBusinessDatabaseCommand, Result>
{
    public async Task<Result> Handle(UpdateBusinessDatabaseCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return Result.Failure("BusinessDatabase not found.");

        existing.UpdateInfo(request.Name, request.Description);
        existing.UpdateConnection(request.ConnectionString, request.Provider);
        existing.SetEnabled(request.IsEnabled);

        repo.Update(existing);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
