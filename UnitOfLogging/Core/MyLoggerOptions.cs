using Microsoft.Extensions.Options;
using NLog.Config;
using System.Collections.Generic;
using UnitOfLogging.Core.TargetConf;
using UnitOfLogging.Models.ExplicitConfiguration;
using UnitOfLogging.Tools;

namespace UnitOfLogging.Core
{
    public sealed class MyLoggerOptions
    {
        private bool Default = false;

        private bool UsingJsonSettings =false;
        private TargetsOptions Targets;
        private LoggingConfiguration _Loggerconfig;
        private LoggingSettings? _JsonSettings;

        private Dictionary<LoggingTarget, string> _LoggersNamesDictionary;

        public MyLoggerOptions(Dictionary<LoggingTarget, string> dictionary) => PreLoadConfiguration(dictionary);
        public MyLoggerOptions(Dictionary<LoggingTarget, string> dictionary, LoggingSettings JsonSettings, bool json)
        {
            PreLoadConfiguration(dictionary);
             _JsonSettings = JsonSettings;
            UsingJsonSettings =json;
        }
        private void PreLoadConfiguration(Dictionary<LoggingTarget, string> dictionary)
        {
            _Loggerconfig = new();
            _LoggersNamesDictionary = dictionary;
            Targets = new TargetsOptions();
        }

       

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
        

        public MyLoggerOptions UseDefaultPresets(Action<TargetsOptions> configureOptions = null!)
        {
            this.Default = true;

            var Options = new TargetsOptions();          

            if (configureOptions is not null) configureOptions(Options);         

            if (UsingJsonSettings && _JsonSettings is not null)
            {
                Options.ConsoleLog = _JsonSettings.Loggers[0].Console.Active;
                Options.FileLog = _JsonSettings.Loggers[0].File.Active;
                Options.SeqLog = _JsonSettings.Loggers[0].Seq.Active;               
            }

            SetDefaultTargetConfiguration(Options);

            return this;
        }


        //este metodo ebe moverse
        private void SetDefaultTargetConfiguration(TargetsOptions Options)
        {
            var Config = new TargetsPrivateConfiguration();
            if (Options.ConsoleLog)
            {               
                this.Targets.ConsoleLog = true;
                _Loggerconfig = Config.AddDefaultColoredConsoleTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.Console));
            }

            if (Options.FileLog)
            {
                this.Targets.FileLog = true;
                _Loggerconfig = Config.AddDefaultFileTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.File));
            }

            if (Options.SeqLog)
            {
                this.Targets.SeqLog = true;
                _Loggerconfig = Config.AddSeqTarget(_Loggerconfig, Config.BuildDefaultSeqTarget(), ExtensionsMethods.GetKey(_LoggersNamesDictionary, LoggingTarget.Seq));
            }
        }





        public TargetsOptions GetTargets() => this.Targets; 
        public LoggingConfiguration BuildConfig() =>_Loggerconfig;
        public Dictionary<LoggingTarget, string> GetLoggersNames() => _LoggersNamesDictionary;
        public bool IsDefault() => this.Default;
    }
}
