using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zilor.AICopilot.DataAnalysisService.Commands.BusinessDatabases;
using Zilor.AICopilot.DataAnalysisService.Queries.BusinessDatabases;
using Zilor.AICopilot.HttpApi.Infrastructure;

namespace Zilor.AICopilot.HttpApi.Controllers;

[Route("/api/data-analysis")]
[Authorize]
public class DataAnalysisController : ApiControllerBase
{
    [HttpGet("business-database/list")]
    public async Task<IActionResult> GetListBusinessDatabases()
    {
        var result = await Sender.Send(new GetListBusinessDatabasesQuery());
        return ReturnResult(result);
    }

    [HttpPost("business-database")]
    public async Task<IActionResult> CreateBusinessDatabase(CreateBusinessDatabaseCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpPut("business-database")]
    public async Task<IActionResult> UpdateBusinessDatabase(UpdateBusinessDatabaseCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }

    [HttpDelete("business-database")]
    public async Task<IActionResult> DeleteBusinessDatabase(DeleteBusinessDatabaseCommand command)
    {
        var result = await Sender.Send(command);
        return ReturnResult(result);
    }
}
