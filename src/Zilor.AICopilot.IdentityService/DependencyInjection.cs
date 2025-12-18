using System.Reflection;
using Microsoft.Extensions.Hosting;
using Zilor.AICopilot.AgentPlugin;

namespace Zilor.AICopilot.IdentityService;

public static class DependencyInjection
{
    public static void AddIdentityService(this IHostApplicationBuilder builder)
    {
        
        builder.Services.AddAgentPlugin(registrar =>
        {
            registrar.RegisterPluginFromAssembly(Assembly.GetExecutingAssembly());
        });
        
    }
}