using System;
using System.ComponentModel;
using Zilor.AICopilot.AgentPlugin;

namespace Zilor.AICopilot.AiGatewayService.Plugins;

public class TimePlugin : AgentPluginBase
{
    [Description("获取当前系统时间")]
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}