using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.Rag.Aggregates.KnowledgeBase;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Commands.KnowledgeBases;

[AuthorizeRequirement("Rag.DeleteKnowledgeBase")]
public record DeleteKnowledgeBaseCommand(Guid Id) : ICommand<Result>;

public class DeleteKnowledgeBaseCommandHandler(IRepository<KnowledgeBase> repo)
    : ICommandHandler<DeleteKnowledgeBaseCommand, Result>
{
    public async Task<Result> Handle(DeleteKnowledgeBaseCommand request, CancellationToken cancellationToken)
    {
        var kb = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (kb == null) return Result.Success();

        repo.Delete(kb);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
