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
        private enum FromSettings
        {
            AppSettings,
            Mixed,
            Code
        }
        //properties

        private bool DefaultPresets = true;
        private bool SettingsFromJson = false;
        private FromSettings SettingsFrom = FromSettings.Code;
        private bool ConfigureLaunched = false;
        private bool IsLoggingActive = false;
        private bool IsConfigurationComplete = false;
        private LoggingSettings? _JsonSettings;


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


        public UnitOfLog UseJsonSettings(IConfiguration configuration, string SectionPart, bool IsActive = true)
        {
            if (string.IsNullOrEmpty(SectionPart))
            {
                ArgumentNullException argumentNullException = new(nameof(SectionPart), "No name was given.");
                throw argumentNullException;
            }
            SettingsFromJson = true;
            this.SettingsFrom = FromSettings.AppSettings;
            _JsonSettings = new LoggingSettings();
            configuration.GetSection(SectionPart).Bind(_JsonSettings);

            if (_JsonSettings is null || _JsonSettings.Loggers is null || !_JsonSettings.Loggers!.Any())
            {
                ArgumentNullException argumentNullException = new(nameof(SectionPart), "AppSettings was no configured correctly");
                throw argumentNullException;
            }

            return this;
        }


        public UnitOfLog Configure(Action<MyLoggerOptions> configureOptions = null!)
        {           
            if(this.SettingsFrom.Equals(FromSettings.AppSettings) && (_JsonSettings is null || _JsonSettings.Loggers is null)) { 
                ArgumentNullException argumentNullException = new(nameof(Configure), "AppSettings was no configured correctly");
                throw argumentNullException; 
            }        

            this.ConfigureLaunched = true;
            this.IsLoggingActive = true;

            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
              
            });                   
            
            if(configureOptions != null)
            {
                _LoggersNames = SetRuleTargetsName();
                var options = new MyLoggerOptions(_LoggersNames);
                if (this.SettingsFromJson)
                {
                    options = new MyLoggerOptions(_LoggersNames, _JsonSettings!, SettingsFromJson);
                }

                configureOptions(options);

                this.DefaultPresets = options.IsDefault();
                if (!this.DefaultPresets) 
                {                     
                    _LoggersNames = options.GetLoggersNames();
                }  
                _Loggerconfig = options.BuildConfig();
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
                var MyLoggerManager = new LoggerManager(provider.GetService<ILoggerFactory>()!, _JsonSettings!);
                MyLoggerManager.AddDefaultLoggers(provider.GetService<ILoggerFactory>()!, _LoggersNames);
                return MyLoggerManager;
            });

            LogManager.Configuration = _Loggerconfig;

            return this;
        }

      

        private Dictionary<LoggingTarget, string> SetRuleTargetsName()
        {
            _LoggersNames.Clear();

            if (this.SettingsFromJson)
            {    
                if (_JsonSettings?.Loggers is null)
                {
                    throw new Exception("No logger config was founded in AppSettings.json");
                }

                foreach (var Logger in _JsonSettings!.Loggers)
                {
                    if (Logger.Active)
                    {
                        _LoggersNames.Add(Logger.Target, Logger.Name);
                    }
                }
            }
            return _LoggersNames;
        }
    }
}






// this.IsConfigurationComplete = true;

// _Services.Configure<LoggingSettings>(configuration.GetSection(SectionPart)); 