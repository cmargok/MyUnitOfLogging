using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit
{

    
    public static class TargetingConfig
    {
        public static void AddRule(this LoggingConfiguration loggerconfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            loggerconfig.AddTarget(TargetName, target);
            loggerconfig.LoggingRules.Add(new LoggingRule(LoggerName, level, target));
        }

        public static LoggingConfiguration AddRuleAsync(this LoggingConfiguration loggerconfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            var asyncFileTarget = new AsyncTargetWrapper(target);
            loggerconfig.AddTarget(TargetName, asyncFileTarget);
            loggerconfig.LoggingRules.Add(new LoggingRule(LoggerName, level, asyncFileTarget));
            return loggerconfig;
        }

    }
    
}
