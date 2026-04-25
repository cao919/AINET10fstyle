using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.Rag.Aggregates.EmbeddingModel;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Commands.EmbeddingModels;

[AuthorizeRequirement("Rag.UpdateEmbeddingModel")]
public record UpdateEmbeddingModelCommand(
    Guid Id,
    string Name,
    string Provider,
    string BaseUrl,
    string? ApiKey,
    string ModelName,
    int Dimensions,
    int MaxTokens) : ICommand<Result>;

public class UpdateEmbeddingModelCommandHandler(IRepository<EmbeddingModel> repo)
    : ICommandHandler<UpdateEmbeddingModelCommand, Result>
{
    public async Task<Result> Handle(UpdateEmbeddingModelCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return Result.Failure("EmbeddingModel not found.");

        existing.Name = request.Name;
        existing.Provider = request.Provider;
        existing.BaseUrl = request.BaseUrl;
        existing.ApiKey = request.ApiKey;
        existing.ModelName = request.ModelName;
        existing.Dimensions = request.Dimensions;
        existing.MaxTokens = request.MaxTokens;

        repo.Update(existing);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
