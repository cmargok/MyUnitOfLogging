using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Xml.Linq;
using UnitOfLogging.Core.TargetConf;
using UnitOfLogging.Logging;
using UnitOfLogging.Models.ExplicitConfiguration;
using UnitOfLogging.Tools;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging.Core
{

    public class UnitOfLog
    {
        private LoggingSettings? _loggingSettings;
        private LoggingConfiguration _Loggerconfig;
        private readonly IServiceCollection _Services;
        private Dictionary<LoggingTarget, string> _LoggersNames;
        private List<string> DefaultLogsNames;
        public UnitOfLog(IServiceCollection services)
        {
            _Services = services;
            _Loggerconfig = new LoggingConfiguration();
            _LoggersNames = new Dictionary<LoggingTarget, string>();
            DefaultLogsNames = new List<string>();
        }

        public UnitOfLog UseMyUnitOfLogging(IConfiguration configuration, string SectionPart, Action<LoggerManagerOptions> configureOptions = null!)
        {           

            if (string.IsNullOrEmpty(SectionPart))
            {
                ArgumentNullException argumentNullException = new (nameof(SectionPart), "No name was given.");
                throw argumentNullException;
            }


            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
              
            }); 

            _loggingSettings = new LoggingSettings();

            configuration.GetSection(SectionPart).Bind(_loggingSettings);

            _LoggersNames = SetRuleTargetsName(_LoggersNames);

            if(configureOptions != null)
            {
                var options = new LoggerManagerOptions(_LoggersNames);
                configureOptions(options);

                // _Services.Configure<LoggingSettings>(configuration.GetSection(SectionPart));   
                _Loggerconfig = options.BuildConfig();
            }





           

          

            return this;
        }

        
        


        public UnitOfLog UseDefaultPresets(Action<TargetsOptions> configureOptions = null!)
        {
            var Config = new TargetsPrivateConfiguration();

            var TargetsOptions = new TargetsOptions();

            if (configureOptions is not  null)
            {
                configureOptions(TargetsOptions);
            }           

            if (TargetsOptions.ConsoleLog)
            {
                _Loggerconfig = Config.AddDefaultColoredConsoleTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNames,LoggingTarget.Console));
            }

            if (TargetsOptions.FileLog)
            {
                _Loggerconfig = Config.AddDefaultFileTarget(_Loggerconfig, ExtensionsMethods.GetKey(_LoggersNames,LoggingTarget.File));
            }

            if (TargetsOptions.SeqLog)
            {
                var seq = Config.BuildDefaultSeqTarget();
                _Loggerconfig = Config.AddSeqTarget(_Loggerconfig, seq, ExtensionsMethods.GetKey(_LoggersNames,LoggingTarget.Seq));
                
            }


            return this;
        }

        public UnitOfLog InitLoggers()
        {
            _Services.AddLogging(logging =>
            {
                logging.AddNLog();
            });
            _Services.AddScoped<IMyLogger>(provider =>
            {
                var MyLoggerManager = new LoggerManager(provider.GetService<ILoggerFactory>()!, _loggingSettings!);
                MyLoggerManager.AddDefaultLoggers(provider.GetService<ILoggerFactory>()!, _LoggersNames);
                return MyLoggerManager;
            });

            LogManager.Configuration = _Loggerconfig;

            return this;
        }

      

        private Dictionary<LoggingTarget, string> SetRuleTargetsName(Dictionary<LoggingTarget, string> LoggersNames)
        {

            LoggersNames.Clear();

            if (_loggingSettings?.Loggers is null)
            {
              //  LoggersNames.Add(LoggingTarget.Console, "ConsoleLogger");
                return LoggersNames;
            }

            foreach (var Logger in _loggingSettings!.Loggers)
            {
                if (Logger.Active)
                {
                    LoggersNames.Add(Logger.Target, Logger.Name);
                }
            }

            return LoggersNames;
        }

    }
}
