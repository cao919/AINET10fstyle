using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zilor.AICopilot.HttpApi.Infrastructure;
using Zilor.AICopilot.RagService.Commands.Documents;
using Zilor.AICopilot.RagService.Commands.EmbeddingModels;
using Zilor.AICopilot.RagService.Commands.KnowledgeBases;
using Zilor.AICopilot.RagService.Queries.EmbeddingModels;
using Zilor.AICopilot.RagService.Queries.KnowledgeBases;

namespace Zilor.AICopilot.HttpApi.Controllers;

[Route("/api/rag")]
[Authorize]
public class RagController : ApiControllerBase
{
    [HttpGet("knowledge-base/list")]
    public async Task<IActionResult> GetListKnowledgeBases()
    {
        var result = await Sender.Send(new GetListKnowledgeBasesQuery());
        return ReturnResult(result);
    }

    [HttpPost("knowledge-base")]
    public async Task<IActionResult> CreateKnowledgeBase(CreateKnowledgeBaseCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpDelete("knowledge-base")]
    public async Task<IActionResult> DeleteKnowledgeBase(DeleteKnowledgeBaseCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpPost("document")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadDocument(
        [FromForm] Guid knowledgeBaseId,
        IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest(new { error = "请选择文件" });
        }

        await using var stream = file.OpenReadStream();

        var command = new UploadDocumentCommand(
            knowledgeBaseId,
            new FileUploadStream(file.FileName, stream));

        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpDelete("document")]
    public async Task<IActionResult> DeleteDocument(DeleteDocumentCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(SearchKnowledgeBaseQuery query)
    {
        var result = await Sender.Send(query);
        return ReturnResult(result);
    }

    [HttpGet("embedding-model/list")]
    public async Task<IActionResult> GetListEmbeddingModels()
    {
        var result = await Sender.Send(new GetListEmbeddingModelsQuery());
        return ReturnResult(result);
    }

    [HttpPost("embedding-model")]
    public async Task<IActionResult> CreateEmbeddingModel(CreateEmbeddingModelCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpPut("embedding-model")]
    public async Task<IActionResult> UpdateEmbeddingModel(UpdateEmbeddingModelCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpDelete("embedding-model")]
    public async Task<IActionResult> DeleteEmbeddingModel(DeleteEmbeddingModelCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }
}
