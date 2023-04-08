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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return new UnitOfLogging(services);
        }
    }

    public class UnitOfLogging {


        private LoggingSettings? _loggingSettings;
        private LoggingConfiguration _Loggerconfig;
        private readonly IServiceCollection _Services;
        private Dictionary<LoggingTarget, string> _LoggersNames;


        public UnitOfLogging(IServiceCollection services)
        {
            _Services = services;
            _Loggerconfig = new LoggingConfiguration();
            _LoggersNames = new Dictionary<LoggingTarget, string>();
        }

        public UnitOfLogging UseMyUnitOfLogging(IConfiguration configuration, Action<LoggerManagerOptions> configureOptions)
        {
            if (configureOptions is null) throw new ArgumentNullException(nameof(UseMyUnitOfLogging));

            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            });
            var options = new LoggerManagerOptions();
            configureOptions(options);

            if (string.IsNullOrEmpty(options.LogSectionName))
            {
                ArgumentNullException argumentNullException = new("No name was given.", nameof(options.LogSectionName));
                throw argumentNullException;
            }

            _Services.Configure<LoggingSettings>(configuration.GetSection(options.LogSectionName));

            _loggingSettings = new LoggingSettings();
            configuration.GetSection(options.LogSectionName).Bind(_loggingSettings);

            _LoggersNames = SetRuleTargetsName(_LoggersNames);  

            options.SetDictionary(_LoggersNames);                

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
                        _Loggerconfig = targetsConfig.AddDefaultColoredConsoleTarget(_Loggerconfig, _LoggersNames[LoggingTarget.Console]);
                    }
                    else
                    {
                        _Loggerconfig = targetsConfig.AddCustomConsoleTarget(_Loggerconfig, options.ConsoleConfiguration.ConsoleTargetConfig!, _LoggersNames[LoggingTarget.Console]);
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
                    _Loggerconfig = Config.AddDefaultColoredConsoleTarget(_Loggerconfig, _LoggersNames[LoggingTarget.Console]);
                }

                if (TargetsOptions.FileLog)
                {
                    _Loggerconfig = Config.AddDefaultFileTarget(_Loggerconfig, _LoggersNames[LoggingTarget.File]);
                }

                if (TargetsOptions.SeqLog)
                {
                    _Loggerconfig = Config.AddDefaultSeqTarget(_Loggerconfig, _LoggersNames[LoggingTarget.Seq]);
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




        private Dictionary<LoggingTarget, string> SetRuleTargetsName(Dictionary<LoggingTarget, string> LoggersNames)
        {

            LoggersNames.Clear();

            if (_loggingSettings?.Loggers.Count == 0)
            {
                LoggersNames.Add(LoggingTarget.Console, "ConsoleLogger");
                return LoggersNames;
            }

            foreach (LoggingTarget target in Enum.GetValues(typeof(LoggingTarget)))
            {
                var name = _loggingSettings?.Loggers.FirstOrDefault(c => c.Target == target)?.Name ?? $"{target}Logger";
                LoggersNames.Add(target, name);
            }
            return LoggersNames;
        }

    }












    public sealed class LoggerManagerOptions
    {
        public string LogSectionName { get; set; }

        private LoggingConfiguration _Loggerconfig;

        private Dictionary<LoggingTarget, string> _LoggersNames;


        public LoggerManagerOptions()
        {

            _Loggerconfig = new();
        }




        public void AddTargets(Action<MyCustomLoggingConfiguration> Targets = null!)
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
                        _Loggerconfig = targetsConfig.AddDefaultColoredConsoleTarget(_Loggerconfig, _LoggersNames[LoggingTarget.Console]);
                    }
                    else
                    {
                        _Loggerconfig = targetsConfig.AddCustomConsoleTarget(_Loggerconfig, options.ConsoleConfiguration.ConsoleTargetConfig!, _LoggersNames[LoggingTarget.Console]);
                    }
                }
            }
        }
        public void SetDictionary(Dictionary<LoggingTarget, string> data)
        {
            _LoggersNames = data;
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
