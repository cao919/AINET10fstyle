using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Queries.EmbeddingModels;

public record EmbeddingModelDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Provider { get; set; }
    public required string BaseUrl { get; set; }
    public string? ApiKey { get; set; }
    public required string ModelName { get; set; }
    public int Dimensions { get; set; }
    public int MaxTokens { get; set; }
    public bool IsEnabled { get; set; }
}

[AuthorizeRequirement("Rag.GetListEmbeddingModels")]
public record GetListEmbeddingModelsQuery : IQuery<Result<IList<EmbeddingModelDto>>>;

public class GetListEmbeddingModelsQueryHandler(
    IDataQueryService dataQueryService)
    : IQueryHandler<GetListEmbeddingModelsQuery, Result<IList<EmbeddingModelDto>>>
{
    public async Task<Result<IList<EmbeddingModelDto>>> Handle(GetListEmbeddingModelsQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = dataQueryService.EmbeddingModels
            .Select(em => new EmbeddingModelDto
            {
                Id = em.Id,
                Name = em.Name,
                Provider = em.Provider,
                BaseUrl = em.BaseUrl,
                ApiKey = em.ApiKey,
                ModelName = em.ModelName,
                Dimensions = em.Dimensions,
                MaxTokens = em.MaxTokens,
                IsEnabled = em.IsEnabled
            });
        var result = await dataQueryService.ToListAsync(queryable);
        return Result.Success(result);
    }
}
