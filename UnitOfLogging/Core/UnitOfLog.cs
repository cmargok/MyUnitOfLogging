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
        private bool IsLoggingActive = true;
        private bool IsConfigurationComplete = false;


        private LoggingSettings? _JsonSettings;

        private LoggingConfiguration _NlogCustomConfiguration;
        private readonly IServiceCollection _Services;
        private Dictionary<LoggingTarget, string> _LoggersAgents;

      //  private List<string> DefaultLogsNames;

        public UnitOfLog(IServiceCollection services) => _Services = services;

        public UnitOfLog(IServiceCollection services, IConfigurationSection ConfigSettings)
        {
            _Services = services;
            PreLoadConfiguration();
            _JsonSettings = new LoggingSettings();
            UseJsonSettings(ConfigSettings);
        }

        private void PreLoadConfiguration()
        {
            _NlogCustomConfiguration = new LoggingConfiguration();
           // DefaultLogsNames = new List<string>();
            _LoggersAgents = new Dictionary<LoggingTarget, string>();
        }

        private void UseJsonSettings(IConfigurationSection ConfigSettings)
        {           
            SettingsFromJson = true;
            this.SettingsFrom = FromSettings.AppSettings;

            ConfigSettings.Bind(_JsonSettings);

            if (_JsonSettings is null)
            {
                ArgumentNullException argumentNullException = new(nameof(UseJsonSettings), "LogSettings json section was no configured correctly");
                throw argumentNullException;
            }
            _LoggersAgents = DefineLoggerAgentsFromJsonSettings();
        }

     


        public UnitOfLog Configure(Action<MyLoggerOptions> configureOptions)
        { 
            this.ConfigureLaunched = true;

            if(configureOptions is null)
            {
                ArgumentNullException argumentNullException = new(nameof(Configure), "configureOptions cannot be null");
                throw argumentNullException;
            }

            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);              
            });                   
             

            var options = new MyLoggerOptions(_LoggersAgents);

            if (this.SettingsFromJson)
            {
                options = new MyLoggerOptions(_LoggersAgents, _JsonSettings!, SettingsFromJson);
            }

            configureOptions(options);

            this.DefaultPresets = options.IsDefault();

            if (!this.DefaultPresets) 
            {                     
                _LoggersAgents = options.GetLoggersNames();
            }  

            _NlogCustomConfiguration = options.BuildConfig();         

            return this;
        } 
        private Dictionary<LoggingTarget, string> DefineLoggerAgentsFromJsonSettings()
        {
            _LoggersAgents.Clear();

            if (this.SettingsFromJson)
            {    
                foreach (var Logger in _JsonSettings!.GetActiveLoggers())
                {
                    if (Logger.Active)
                    {
                        _LoggersAgents.Add(Logger.Target, Logger.Name);
                    }
                }
            }
            return _LoggersAgents;
        }


        public UnitOfLog InitLoggers()
        {
            _Services.AddLogging(logging => logging.AddNLog());
            /*
            _Services.AddScoped<IMyLogger>(provider =>
            {
                var MyLoggerManager = new LoggerManager(provider.GetService<ILoggerFactory>()!, _JsonSettings!);
                MyLoggerManager.AddLoggers(!, _LoggersAgents);
                return MyLoggerManager;
            });
            */
            _Services.AddScoped<IMyLogger,LoggerManager>(provider =>
            {
                var instance = new LoggerManager();
                if (IsLoggingActive)
                {
                    instance.Activate();
                    var Loggers = AddMyLoggers(provider.GetService<ILoggerFactory>()!);
                    instance.SetLoggers(Loggers);
                }
                return instance;
            });
            LogManager.Configuration = _NlogCustomConfiguration;

            return this;
        }

        private List<IMyLogger> AddMyLoggers(ILoggerFactory loggerFactory)
        {
            if(loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            List<IMyLogger> mylist = new();
            
            foreach (var logger in _LoggersAgents)
            {
                mylist.Add(new MyLogger(loggerFactory, logger.Value));
            }
           
            return mylist;
        }
    }
}






// this.IsConfigurationComplete = true;

// _Services.Configure<LoggingSettings>(configuration.GetSection(SectionPart)); 