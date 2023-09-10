using MyLoggingUnit.Tools;
using NLog.Config;
using NLog.Targets;
using static MyLoggingUnit.TargetingOptionsConfiguration;

namespace MyLoggingUnit
{
    public sealed class LoggerOptions
    {
        private bool Default = false;

        //private bool UsingJsonSettings = false;
        //private TargetsOptions Targets;
        //private LoggingConfiguration _Loggerconfig;
        //private LoggingSettings? _JsonSettings;

        //private Dictionary<LoggingTarget, string> _LoggersNamesDictionary;

        //public LoggerOptions(Dictionary<LoggingTarget, string> dictionary) => PreLoadConfiguration(dictionary);

        //public LoggerOptions(Dictionary<LoggingTarget, string> dictionary, LoggingSettings JsonSettings, bool json)
        //{
        //    PreLoadConfiguration(dictionary);
        //    _JsonSettings = JsonSettings;
        //    UsingJsonSettings = json;
        //}
        //private void PreLoadConfiguration(Dictionary<LoggingTarget, string> dictionary)
        //{
        //    _Loggerconfig = new();
        //    _LoggersNamesDictionary = dictionary;
        //    Targets = new TargetsOptions();
        //}


        /*
        public LoggerOptions AddTargets(Action<TargetingOptionsConfiguration> Targets = null!)
        {
            if (!_LoggersNamesDictionary.Any()) throw new Exception("No loggers registered");

            _Loggerconfig = new LoggingConfiguration();
            if (Targets != null)
            {
                var options = new TargetingOptionsConfiguration();
                options.SetDictionary(_LoggersNamesDictionary);
                Targets(options);
                _Loggerconfig = options.GetLogConfiguration();
            }

            return this;
        }*/


        public LoggerOptions UseDefaultPresets(/*Action<TargetsOptions> configureOptions = null!*/)
        {
            this.Default = true;

           /* var Options = new TargetsOptions();

           // if (configureOptions is not null) configureOptions(Options);

            if (UsingJsonSettings && _JsonSettings is not null)
            {
                Options.ConsoleLog = _JsonSettings.Loggers[0].Console.Active;
                Options.FileLog = _JsonSettings.Loggers[0].File.Active;
                Options.SeqLog = _JsonSettings.Loggers[0].Seq.Active;
            }

            SetDefaultTargetConfiguration(Options);*/

            return this;
        }
                





      //  public TargetsOptions GetTargets() => this.Targets;
      //  public LoggingConfiguration BuildConfig() => _Loggerconfig;
        //public Dictionary<LoggingTarget, string> GetLoggersNames() => _LoggersNamesDictionary;


        public bool IsDefault() => this.Default;
    }


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
            var Config = new TargetsPrivateConfiguration();

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

            if (loggersSet.Seq.Active)
            {
              //  this.Targets.SeqLog = true;
                Config.AddSeqTarget(loggerConfig, Config.BuildDefaultSeqTarget(), loggersSet.Seq.Name);
            }
        }

    }


}
