using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.Rag.Aggregates.EmbeddingModel;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Commands.EmbeddingModels;

public record CreatedEmbeddingModelDto(Guid Id, string Name);

[AuthorizeRequirement("Rag.CreateEmbeddingModel")]
public record CreateEmbeddingModelCommand(
    string Name,
    string Provider,
    string BaseUrl,
    string? ApiKey,
    string ModelName,
    int Dimensions,
    int MaxTokens) : ICommand<Result<CreatedEmbeddingModelDto>>;

public class CreateEmbeddingModelCommandHandler(IRepository<EmbeddingModel> repo)
    : ICommandHandler<CreateEmbeddingModelCommand, Result<CreatedEmbeddingModelDto>>
{
    public async Task<Result<CreatedEmbeddingModelDto>> Handle(CreateEmbeddingModelCommand request,
        CancellationToken cancellationToken)
    {
        var model = new EmbeddingModel(
            request.Name,
            request.Provider,
            request.BaseUrl,
            request.ModelName,
            request.Dimensions,
            request.MaxTokens);

        if (!string.IsNullOrEmpty(request.ApiKey))
        {
            model.ApiKey = request.ApiKey;
        }

        repo.Add(model);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreatedEmbeddingModelDto(model.Id, model.Name));
    }
}
