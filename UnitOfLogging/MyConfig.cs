using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Xml.Linq;
using UnitOfLogging.Logging;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging
{
    public static class LoggingUnitOfMeasurement
    {
        public static UnitOfLogging UseUnitOfLogging(this IServiceCollection services)
        {
            return new UnitOfLogging(services);
        }
    }

    public class UnitOfLogging {

       
        private LoggingSettings? _loggingSettings;
        private LoggingConfiguration _Loggerconfig;
        private readonly IServiceCollection _Services;
        private Dictionary<LoggingTarget, string> LoggersNames;
        public UnitOfLogging(IServiceCollection services)
        {
            _Services = services;
            _Loggerconfig = new LoggingConfiguration(); 
        }

        public UnitOfLogging UseMyUnitOfLogging(IConfiguration configuration, Action<LoggerManagerOptions> configureOptions = null)
        {

            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            });
            if(configureOptions != null) {

                var options = new LoggerManagerOptions();
                configureOptions(options);

                if (String.IsNullOrEmpty(options.LogSectionName))
                {
                    ArgumentNullException argumentNullException = new("No name was given.", nameof(options.LogSectionName));
                    throw argumentNullException;
                }

                _Services.Configure<LoggingSettings>(configuration.GetSection(options.LogSectionName));
                _loggingSettings = new LoggingSettings();
                configuration.GetSection(options.LogSectionName).Bind(_loggingSettings);
            }
            SetRuleTargetsName();
            _Services.AddSingleton<IApiLogger, LoggerManager>();
            return this;
        }

       
        
        public UnitOfLogging AddTargets(Action<MyCustomLoggingConfiguration> Targets = null!)
        {
            if (Targets != null)
            {
                var targetsConfig = new TargetsConfiguration();
                var options = new MyCustomLoggingConfiguration();
                Targets(options);



                if (options.ConsoleConfiguration.ConsoleLog)
                {

                    if (options.DefaultConsoleLogSettings)
                    {
                        _Loggerconfig = targetsConfig.AddDefaultColoredConsoleTarget(_Loggerconfig, LoggersNames[LoggingTarget.Console]);
                    }
                    else
                    {
                        _Loggerconfig = targetsConfig.AddCustomConsoleTarget(_Loggerconfig, options.ConsoleConfiguration.ConsoleTargetConfig!, LoggersNames[LoggingTarget.Console]);
                    }
                }





            }
            SetConfig();
            return this;
        }

        public UnitOfLogging UseDefaultPresets(Action<TargetsOptions> configureOptions = null!)
        {
            var Config = new TargetsConfiguration();

            if (configureOptions != null)
            { 
                var TargetsOptions = new TargetsOptions();
                configureOptions(TargetsOptions);

                if (TargetsOptions.ConsoleLog)
                {   
                    _Loggerconfig = Config.AddDefaultColoredConsoleTarget(_Loggerconfig, LoggersNames[LoggingTarget.Console]);
                }

                if (TargetsOptions.FileLog)
                {
                    _Loggerconfig = Config.AddDefaultFileTarget(_Loggerconfig, LoggersNames[LoggingTarget.File]);
                }

                if (TargetsOptions.SeqLog)
                {
                    _Loggerconfig = Config.AddDefaultSeqTarget(_Loggerconfig, LoggersNames[LoggingTarget.Seq]);
                }

            }
            SetConfig();
            return this;
        }



        public UnitOfLogging RenderMagic()
        {
            return this;
        }






        private void SetConfig()
        {
            LogManager.Configuration = _Loggerconfig;
        }




        private void SetRuleTargetsName()
        {
            LoggersNames = new();

            if (_loggingSettings?.Loggers.Count  == 0)
            {
                LoggersNames.Add(LoggingTarget.Console, "ConsoleLogger");
                return;
            }

            foreach (LoggingTarget target in Enum.GetValues(typeof(LoggingTarget)))
            {
                var name = _loggingSettings?.Loggers.FirstOrDefault(c => c.Target == target)?.Name ?? $"{target}Logger";
                LoggersNames.Add(target, name);
            }

        }

    }












    public sealed class LoggerManagerOptions
    {
        public string LogSectionName { get; set; }

        private LoggingConfiguration _Loggerconfig;

        private Dictionary<LoggingTarget, string> LoggersNames;
        public void AddTargets(Action<MyCustomLoggingConfiguration> Targets = null!, Dictionary<LoggingTarget, string> LoggersNames = null!)
        {
            _Loggerconfig = new LoggingConfiguration();
            if (Targets != null)
            {
                var targetsConfig = new TargetsConfiguration();
                var options = new MyCustomLoggingConfiguration();
                Targets(options);



                if (options.ConsoleConfiguration.ConsoleLog)
                {

                    if (options.DefaultConsoleLogSettings)
                    {
                        _Loggerconfig = targetsConfig.AddDefaultColoredConsoleTarget(_Loggerconfig, LoggersNames[LoggingTarget.Console]);
                    }
                    else
                    {
                        _Loggerconfig = targetsConfig.AddCustomConsoleTarget(_Loggerconfig, options.ConsoleConfiguration.ConsoleTargetConfig!, LoggersNames[LoggingTarget.Console]);
                    }
                }
            }
        }


        public LoggingConfiguration BuildConfig()
        {
            return _Loggerconfig;
        }


    }



    public class TargetsOptions
    {
        public bool ConsoleLog { get; set; } = true;
        public bool FileLog { get; set; } = true;
        public bool SeqLog { get; set; } = true;

    }

}
