using Zilor.AICopilot.SharedKernel.Domain;

namespace Zilor.AICopilot.Core.AiGateway.Aggregates.LanguageModel;

public class LanguageModel : IAggregateRoot
{
    protected LanguageModel()
    {
    }

    public LanguageModel(Guid? id, string name, string provider, string baseUrl, string? apiKey, ModelParameters parameters)
    {
        Id =id?? Guid.NewGuid();
        Name = name;
        Provider = provider;
        BaseUrl = baseUrl;
        ApiKey = apiKey;
        Parameters = parameters;
    } 
    public Guid Id { get; set; }

    public string Provider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string? ApiKey { get; set; }

    public ModelParameters Parameters { get; set; } = null!;

    public void UpdateParameters(ModelParameters parameters)
    {
        Parameters = parameters;
    }
}