using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using UnitOfLogging;
using UnitOfLogging.Core;
using UnitOfLogging.Core.TargetConf;
using UnitOfLogging.Logging;
using UnitOfLogging.Models.ExplicitConfiguration;
using UnitOfLogging.Tools;

namespace UnitOfLogging
{
    public static class LoggingUnitOfMeasurement
    {
        public static UnitOfLog AddMyUnitOfLogging(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return new UnitOfLog(services);
        }

        public static UnitOfLogFactory AddMyPerrito(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return UnitOfLogFactory.CreateFactory(services);
        }
    }









    public class UnitOfLogFactory
    {
        private readonly IServiceCollection _Services;
        private LoggingSettings? _JsonConfiguration;
        private TargetsOptionsAsync AsyncOptions;
        private TargetsOptions ActivationOptions;
        private bool ReadyToBuild = false;
        private MymO mim;

        private UnitOfLogFactory(IServiceCollection services) {
            _Services = services;
            mim = new MymO();
            AsyncOptions = new TargetsOptionsAsync();
            ActivationOptions = new TargetsOptions();
        }

        public static UnitOfLogFactory CreateFactory(IServiceCollection services)
        {
            return new UnitOfLogFactory(services);
        }







        public UnitOfLogFactory UseJsonConfiguration(IConfigurationSection ConfigSettings)
        {
            mim.SettingsFromJson = true;
            _JsonConfiguration = ConfigSettings.BindSettings();
            SetAgentsToMymOFromJson(_JsonConfiguration);
            return this;
        }

        public UnitOfLogFactory LoggingAsync(Action<TargetsOptionsAsync> configureOptions = null!)
        {
            var Options = new TargetsOptionsAsync();

            if (configureOptions is not null) configureOptions(Options);

            if (mim.SettingsFromJson)
            {
                Options.FileLog = _JsonConfiguration!.Loggers[0].File.Async;
                Options.SeqLog = _JsonConfiguration!.Loggers[0].Seq.Async;
            }

            if (Options.AllAsync)
            {
                Options.FileLog = true;
                Options.SeqLog = true;
            }

            Options.ConsoleLog = false;

            AsyncOptions = Options;
            return this;
        }

        public UnitOfLogFactory UseDefault(Action<TargetsOptions> configureOptions = null!)
        {
            var Options = new TargetsOptions();

            if (configureOptions is not null) configureOptions(Options);

            if (mim.SettingsFromJson)
            {
                Options.ConsoleLog = _JsonConfiguration!.Loggers[0].Console.Active;
                Options.FileLog = _JsonConfiguration!.Loggers[0].File.Active;
                Options.SeqLog = _JsonConfiguration!.Loggers[0].Seq.Async;
            }

            ActivationOptions = Options;

            this.ReadyToBuild = true;
            return this;

        }






        public MymO Build()
        {
            if (!ReadyToBuild) throw new Exception("Custom or default configuration was no implemented");

            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            });
            BuildTargetOptions();

            if (mim.BuildLoggersConfiguration())
            {
                LogManager.Configuration = mim.BuildConfig();



                RegisterLoggins();
                _Services.AddLogging(logging => logging.AddNLog());
                return mim;
            }

            throw new NotImplementedException();




        }







        private void RegisterLoggins()
        {
            _Services.AddScoped<IMyLogger, LoggerManager>(provider =>
            {
                var instance = new LoggerManager();
                if (mim.IsActive)
                {
                    instance.Activate();
                    var Loggers = AddMyLoggers(provider.GetService<ILoggerFactory>()!);
                    instance.SetLoggers(Loggers);
                }
                return instance;
            });

        }
        private void SetAgentsToMymOFromJson(LoggingSettings settings)
        {
            var agents = mim.GetAgentsDictionary();

            foreach (var Logger in settings.GetActiveLoggers())
            {
                if (Logger.Active)
                {
                    agents.Add(Logger.Target, Logger.Name);
                }
            }

            mim.SetAgents(agents);
        }
        private void BuildTargetOptions()
        {
            var Options = new TargetsActualConfigu();

            if (mim.SettingsFromJson)
            {
                Options.ConsoleLog.IsActive = _JsonConfiguration!.Loggers[0].Console.Active;
                Options.FileLog.IsActive = _JsonConfiguration.Loggers[0].File.Active;
                Options.SeqLog.IsActive = _JsonConfiguration.Loggers[0].Seq.Active;
            }

            Options.ConsoleLog.Async = false;
            Options.FileLog.Async = ActivationOptions.FileLog;
            Options.SeqLog.Async = ActivationOptions.SeqLog;

            mim.targetsActualConfigu = Options;
        }


        private List<IMyLogger> AddMyLoggers(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            List<IMyLogger> mylist = new();

            foreach (var logger in mim.GetAgentsDictionary())
            {
                mylist.Add(new MyLogger(loggerFactory, logger.Value));
            }

            return mylist;
        }













    }




    public class MymO
    {
        public bool DefaultPresets = true;
        public bool SettingsFromJson = false;
        public bool AsyncOverAll = false;
        public bool IsActive = true;
        //aqui creamo sun objeto que hara toda la configuracion por default y mas adelanta la configuracion personalizada

        private LoggingConfiguration _NlogCustomConfiguration;

        private Dictionary<LoggingTarget, string> _LoggersAgents;

        public TargetsActualConfigu targetsActualConfigu;
        public MymO()
        {
            _NlogCustomConfiguration = new LoggingConfiguration();
            _LoggersAgents = new();
            targetsActualConfigu = new TargetsActualConfigu();
        }

        public void SetAgents(Dictionary<LoggingTarget, string> Agents) => _LoggersAgents = Agents;
        public Dictionary<LoggingTarget, string> GetAgentsDictionary() => _LoggersAgents;



        public bool BuildLoggersConfiguration()
        {
            return true;
        }

        public LoggingConfiguration BuildConfig() => _NlogCustomConfiguration;
    }











    public static class JsonConfiguration
    {
        private static string ErrorMessage = "LogSettings json section was no configured correctly";

        public static LoggingSettings BindSettings(this IConfigurationSection ConfigSettings)
        {
            LoggingSettings Settings = new();

            ConfigSettings.Bind(Settings);

            Settings.ThrowIfNull(ErrorMessage);

            if (!(Settings.Loggers.Count > 0)) throw new Exception(ErrorMessage);

            return Settings;
        }
    }


}






