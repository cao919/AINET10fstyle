using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.AiGateway.Aggregates.ConversationTemplate;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.AiGatewayService.Commands.ConversationTemplates;

[AuthorizeRequirement("AiGateway.UpdateConversationTemplate")]
public record UpdateConversationTemplateCommand(
    Guid Id,
    string Name,
    string Description,
    string SystemPrompt,
    Guid ModelId,
    int? MaxTokens,
    float? Temperature) : ICommand<Result>;

public class UpdateConversationTemplateCommandHandler(IRepository<ConversationTemplate> repo)
    : ICommandHandler<UpdateConversationTemplateCommand, Result>
{
    public async Task<Result> Handle(UpdateConversationTemplateCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            return Result.Failure("ConversationTemplate not found.");

        existing.Name = request.Name;
        existing.Description = request.Description;
        existing.SystemPrompt = request.SystemPrompt;
        existing.ModelId = request.ModelId;
        existing.Specification = new TemplateSpecification
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };

        repo.Update(existing);
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
