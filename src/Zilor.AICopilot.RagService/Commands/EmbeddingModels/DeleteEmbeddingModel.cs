using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.Rag.Aggregates.EmbeddingModel;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Commands.EmbeddingModels;

[AuthorizeRequirement("Rag.DeleteEmbeddingModel")]
public record DeleteEmbeddingModelCommand(Guid Id) : ICommand<Result>;

public class DeleteEmbeddingModelCommandHandler(IRepository<EmbeddingModel> repo)
    : ICommandHandler<DeleteEmbeddingModelCommand, Result>
{
    public async Task<Result> Handle(DeleteEmbeddingModelCommand request, CancellationToken cancellationToken)
    {
        var model = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (model == null) return Result.Success();

        repo.Delete(model);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
