using System;
using System.Threading;
using System.Threading.Tasks;
using Zilor.AICopilot.Core.AiGateway.Aggregates.LanguageModel;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.SharedKernel.Messaging;
using Zilor.AICopilot.SharedKernel.Repository;
using Zilor.AICopilot.SharedKernel.Result;

namespace Zilor.AICopilot.AiGatewayService.Commands.LanguageModels;

public record UpdateLanguageModelDto(Guid Id, string Provider, string Name);

[AuthorizeRequirement("AiGateway.UpdateLanguageModel")]
public record UpdateLanguageModelCommand(Guid Id,
    string Provider,
    string Name,
    string BaseUrl,
    string? ApiKey,
    int MaxTokens,
    float Temperature = 0.7f) : ICommand<Result<UpdateLanguageModelDto>>;

public class UpdateLanguageModelCommandHandler(IRepository<LanguageModel> repo)
    : ICommandHandler<UpdateLanguageModelCommand, Result<UpdateLanguageModelDto>>
{
    public async Task<Result<UpdateLanguageModelDto>> Handle(UpdateLanguageModelCommand request,
        CancellationToken cancellationToken)
    {
        //var result = new LanguageModel(
        //    request.
        //    request.Name,
        //    request.Provider,
        //    request.BaseUrl,
        //    request.ApiKey,
        //    new ModelParameters
        //    {
        //        MaxTokens = request.MaxTokens,
        //        Temperature = request.Temperature
        //    });

        //repo.Update(result);
        ////repo.Add(result);

        ////await repo.SaveChangesAsync(cancellationToken);

        //return Result.Success(new UpdateLanguageModelDto(result.Id, result.Provider, result.Name));

        //var isresult = await repo.GetByIdAsync(request.Id, cancellationToken);
        //if (isresult == null) return Result.Success();
        //var result = new LanguageModel(
        //    request.Id, 
        //    request.Name,
        //    request.Provider,
        //    request.BaseUrl,
        //    request.ApiKey,
        //    new ModelParameters
        //    {
        //        MaxTokens = request.MaxTokens,
        //        Temperature = request.Temperature
        //    });
        //repo.Update(result);

        var existingModel = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existingModel == null)
            return Result.Failure("LanguageModel not found.");

        // 更新需要更改的属性
        existingModel.Name = request.Name;
        existingModel.Provider = request.Provider;
        existingModel.BaseUrl = request.BaseUrl;
        existingModel.ApiKey = request.ApiKey;
        existingModel.Parameters.MaxTokens = request.MaxTokens;
        existingModel.Parameters.Temperature = request.Temperature;
        // 如果你的仓储需要显式调用 Update
         repo.Update(existingModel); 
        await repo.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}