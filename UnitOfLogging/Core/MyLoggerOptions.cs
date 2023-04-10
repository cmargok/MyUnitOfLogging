using Microsoft.Extensions.Options;
using NLog.Config;
using UnitOfLogging.Core.TargetConf;
using UnitOfLogging.Models.ExplicitConfiguration;
using UnitOfLogging.Tools;

namespace UnitOfLogging.Core
{
    public sealed class MyLoggerOptions
    {

        private bool Default = false;
        private bool JsonSettingsareOn =false;
        private TargetsOptions Targets;
        private LoggingConfiguration _Loggerconfig;
        private LoggingSettings? _JsonSettings;

        private Dictionary<LoggingTarget, string> _LoggersNamesDictionary;

        public MyLoggerOptions(Dictionary<LoggingTarget, string> dictionary, LoggingSettings JsonSettings, bool json)
        {
            _LoggersNamesDictionary = dictionary;
            _Loggerconfig = new();
            Targets = new TargetsOptions();
            _JsonSettings = JsonSettings;
            JsonSettingsareOn =json;
        }

        public MyLoggerOptions(Dictionary<LoggingTarget, string> dictionary)
        {
            _LoggersNamesDictionary = dictionary;
            _Loggerconfig = new();
            Targets = new TargetsOptions();
        }
        public Dictionary<LoggingTarget, string> GetLoggersNames() => _LoggersNamesDictionary;
        public MyLoggerOptions AddTargets(Action<TargetingOptionsConfiguration> Targets = null!)
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
        }

        public bool IsDefault() => this.Default;

        public MyLoggerOptions UseDefaultPresets(Action<TargetsOptions> configureOptions = null!)
        {
            this.Default = true;

            var Config = new TargetsPrivateConfiguration();

            var TargetsOptions = new TargetsOptions();

            if (configureOptions is not null)
            {
                configureOptions(TargetsOptions);
            }
            var logActive = false;
            var fileActive = false;
            var seqActive = false;

            if (JsonSettingsareOn)
            {
                var list = new List<bool> { logActive, fileActive, seqActive};
                var targets = new List<LoggingTarget>
                {
                    LoggingTarget.Console,
                    LoggingTarget.File,
                    LoggingTarget.Seq
                };

                int counter = 0;

                foreach (var item in list)
                {
                    list[counter] = _JsonSettings.Loggers.Any(c => c.Active && c.Target.Equals(targets[counter]));
                    counter++;
                }
            }
            else
            {
                 logActive = TargetsOptions.ConsoleLog;
                 fileActive = TargetsOptions.FileLog;
                 seqActive = TargetsOptions.SeqLog;
            }
            
            if (logActive)
            {
                if (JsonSettingsareOn )
                    this.Targets.ConsoleLog = true;
                _Loggerconfig = Config.AddDefaultColoredConsoleTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.Console));
            }

            if (fileActive)
            {
                this.Targets.FileLog = true;
                _Loggerconfig = Config.AddDefaultFileTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.File));
            }

            if (seqActive)
            {
                this.Targets.SeqLog = true;
                var seq = Config.BuildDefaultSeqTarget();
                _Loggerconfig = Config.AddSeqTarget(_Loggerconfig, seq, ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.Seq));
            }


            return this;
        }
        public TargetsOptions GetTargets() => this.Targets; 
        public LoggingConfiguration BuildConfig()
        {
            return _Loggerconfig;
        }
    }
}
