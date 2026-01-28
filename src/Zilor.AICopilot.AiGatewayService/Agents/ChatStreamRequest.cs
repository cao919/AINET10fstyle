using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using MediatR;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Zilor.AICopilot.AiGatewayService.Workflows;
using Zilor.AICopilot.Services.Common.Attributes;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.Services.Common.Helper;

namespace Zilor.AICopilot.AiGatewayService.Agents;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChunkType
{
    Error,
    Text,
    FunctionCall,
    FunctionResult
}

public record ChatChunk(string Source, ChunkType Type, string Content);

[AuthorizeRequirement("AiGateway.Chat")]
public record ChatStreamRequest(Guid SessionId, string Message) : IStreamRequest<object>;

public class ChatStreamHandler(
    IDataQueryService queryService, 
    [FromKeyedServices(nameof(IntentWorkflow))]Workflow workflow) 
    : IStreamRequestHandler<ChatStreamRequest, object>
{
    public async IAsyncEnumerable<object> Handle(ChatStreamRequest request, CancellationToken cancellationToken)
    {
        if (!queryService.Sessions.Any(session => session.Id == request.SessionId))
        {
            throw new Exception("未找到会话");
        }
        
        await using var run = await InProcessExecution.StreamAsync(workflow, request, cancellationToken: cancellationToken);
        await foreach (var workflowEvent in run.WatchStreamAsync(cancellationToken))
        {
            switch (workflowEvent)
            {
              case ExecutorFailedEvent evt:
                    yield return new ChatChunk(evt.ExecutorId, ChunkType.Error, evt.Data.Message);
                    break;
                case AgentRunResponseEvent evt:
                    var evtText = evt.Response.Text;
                    if (evt.ExecutorId == nameof(IntentRoutingExecutor))
                    {
                        evtText = $"""
                                   
                                   ```json
                                   {evt.Response.Text}
                                   ```

                                   """;
                    }
                    yield return new ChatChunk(evt.ExecutorId, ChunkType.Text, evtText);
                    break;
                case AgentRunUpdateEvent evt:
                    foreach (var evtContent in evt.Update.Contents)
                    {
                        switch (evtContent)
                        {
                            case TextContent content:
                                yield return new ChatChunk(evt.ExecutorId, ChunkType.Text, content.Text);
                                break;
                            case FunctionCallContent content:
                                var fun = new
                                {
                                    content.Name, content.Arguments
                                };
                                yield return new ChatChunk(evt.ExecutorId, ChunkType.FunctionCall,
                                    $"""
                                    
                                    ```json
                                    {fun.ToJson()}
                                    ```
                                    
                                    """);
                                break;
                            case FunctionResultContent content:
                                yield return new ChatChunk(evt.ExecutorId, ChunkType.FunctionResult, 
                                    $"""
                         
                                     ```
                                     {content.Result}
                                     ```
                                     
                                     """);
                                break;
                        }
                    }
                    break;
            }
        }
    }
}