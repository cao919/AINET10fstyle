using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.DataAnalysis.Aggregates.BusinessDatabase;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.DataAnalysisService.Commands.BusinessDatabases;

[AuthorizeRequirement("DataAnalysis.DeleteBusinessDatabase")]
public record DeleteBusinessDatabaseCommand(Guid Id) : ICommand<Result>;

public class DeleteBusinessDatabaseCommandHandler(IRepository<BusinessDatabase> repo)
    : ICommandHandler<DeleteBusinessDatabaseCommand, Result>
{
    public async Task<Result> Handle(DeleteBusinessDatabaseCommand request, CancellationToken cancellationToken)
    {
        var db = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (db == null) return Result.Success();

        repo.Delete(db);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
