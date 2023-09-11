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

        public static void AddRule(this LoggingConfiguration loggerConfig, string TargetName, LogLevel level, Target target, string LoggerName, bool Async = false)
        { 
            if(Async)
            {
                AddRuleAsync(loggerConfig, TargetName, level, target, LoggerName);
            }
            AddSyncRule(loggerConfig, TargetName, level, target, LoggerName);

        }
        private static void AddSyncRule( LoggingConfiguration loggerConfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            loggerConfig.AddTarget(TargetName, target);
            loggerConfig.LoggingRules.Add(new LoggingRule(LoggerName, level, target));
        }

        private static void AddRuleAsync(LoggingConfiguration loggerConfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            var asyncFileTarget = new AsyncTargetWrapper(target);
            loggerConfig.AddTarget(TargetName, asyncFileTarget);
            loggerConfig.LoggingRules.Add(new LoggingRule(LoggerName, level, asyncFileTarget));
            
        }

    }

}
