using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.RagService.Queries.KnowledgeBases;

public record KnowledgeBaseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public Guid EmbeddingModelId { get; set; }
    public List<DocumentDto> Documents { get; set; } = [];
}

public record DocumentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Extension { get; set; }
    public required string Status { get; set; }
    public int ChunkCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

[AuthorizeRequirement("Rag.GetListKnowledgeBases")]
public record GetListKnowledgeBasesQuery : IQuery<Result<IList<KnowledgeBaseDto>>>;

public class GetListKnowledgeBasesQueryHandler(
    IDataQueryService dataQueryService)
    : IQueryHandler<GetListKnowledgeBasesQuery, Result<IList<KnowledgeBaseDto>>>
{
    public async Task<Result<IList<KnowledgeBaseDto>>> Handle(GetListKnowledgeBasesQuery request,
        CancellationToken cancellationToken)
    {
        var queryable = dataQueryService.KnowledgeBases
            .Select(kb => new KnowledgeBaseDto
            {
                Id = kb.Id,
                Name = kb.Name,
                Description = kb.Description,
                EmbeddingModelId = kb.EmbeddingModelId,
                Documents = kb.Documents.Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Extension = d.Extension,
                    Status = d.Status.ToString(),
                    ChunkCount = d.ChunkCount,
                    CreatedAt = d.CreatedAt
                }).ToList()
            });
        var result = await dataQueryService.ToListAsync(queryable);
        return Result.Success(result);
    }
}
