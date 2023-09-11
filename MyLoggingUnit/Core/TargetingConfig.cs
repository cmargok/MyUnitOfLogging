using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Core
{


    public static class TargetingConfig
    {
        public static void AddRule(this LoggingConfiguration loggerConfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            loggerConfig.AddTarget(TargetName, target);
            loggerConfig.LoggingRules.Add(new LoggingRule(LoggerName, level, target));
        }

        public static LoggingConfiguration AddRuleAsync(this LoggingConfiguration loggerConfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            var asyncFileTarget = new AsyncTargetWrapper(target);
            loggerConfig.AddTarget(TargetName, asyncFileTarget);
            loggerConfig.LoggingRules.Add(new LoggingRule(LoggerName, level, asyncFileTarget));
            return loggerConfig;
        }

    }

}
