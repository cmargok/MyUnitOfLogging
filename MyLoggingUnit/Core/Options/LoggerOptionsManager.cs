using MyLoggingUnit.Core.DefaultConfiguration;
using MyLoggingUnit.Models.Entities;
using MyLoggingUnit.Tools.Enums;
using NLog.Config;

namespace MyLoggingUnit.Core.Options
{
    public class LoggerOptionsManager
    {
        private Dictionary<LoggingTarget, string> _LoggersNamesDictionary = new();
        public void SetDefaultLoggersConfiguration(LoggersSet loggersSet, LoggingConfiguration loggerConfig)
        {
            /*var Options = new TargetsOptions();

            if (loggersSet is not null)
            {
                Options.ConsoleLog = loggersSet.Console.Active;
                Options.FileLog = loggersSet.File.Active;
                Options.SeqLog = loggersSet.Seq.Active;
            }
            */
            SetDefaultTargetConfiguration(loggersSet, loggerConfig);

        }
        private void SetDefaultTargetConfiguration(LoggersSet loggersSet, LoggingConfiguration loggerConfig)
        {
            var Config = new SetDefaultTargetConfiguration();

            if (loggersSet.Console.Active)
            {
                //  this.Targets.ConsoleLog = true;
                Config.AddDefaultColoredConsoleTarget(loggerConfig, loggersSet.Console.Name);
            }

            if (loggersSet.File.Active)
            {
                //  this.Targets.FileLog = true;
                Config.AddDefaultFileTarget(loggerConfig, loggersSet.File.Name);
            }

            if (loggersSet.ErrorFile.Active)
            {
                //  this.Targets.FileLog = true;
                Config.AddDefaultErrorFileTarget(loggerConfig, loggersSet.ErrorFile.Name);
            }

            if (loggersSet.Seq.Active)
            {
                //  this.Targets.SeqLog = true;
                Config.AddSeqTarget(loggerConfig, Config.BuildDefaultSeqTarget(), loggersSet.Seq.Name);
            }
        }

    }


}
