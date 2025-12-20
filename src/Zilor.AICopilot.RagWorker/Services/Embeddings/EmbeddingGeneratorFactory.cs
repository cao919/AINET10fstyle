using System.ClientModel;
using System.ClientModel.Primitives;
using Microsoft.Extensions.AI;
using OpenAI;
using Zilor.AICopilot.Core.Rag.Aggregates.EmbeddingModel;

namespace Zilor.AICopilot.RagWorker.Services.Embeddings;

public class EmbeddingGeneratorFactory(IHttpClientFactory httpClientFactory)
{
    public IEmbeddingGenerator<string, Embedding<float>> CreateGenerator(EmbeddingModel model)
    {
        var endpoint = new Uri(model.BaseUrl);
        var credential = new ApiKeyCredential(model.ApiKey ?? "sk-empty");

        var httpClient = httpClientFactory.CreateClient("EmbeddingClient");
        
        var options = new OpenAIClientOptions
        {
            Endpoint = endpoint,
            // 使用 IHttpClientFactory 创建 HttpClient，复用连接池
            Transport = new HttpClientPipelineTransport(httpClient),
            NetworkTimeout = TimeSpan.FromMinutes(20)
        };

        // 创建 OpenAI 客户端
        var client = new OpenAIClient(credential, options);
        return client
            .GetEmbeddingClient(model.ModelName)
            .AsIEmbeddingGenerator(model.Dimensions);
    }
}