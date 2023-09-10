using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace MyLoggingUnit
{

    public class MyUnitOfLog
    {
        private readonly IServiceCollection _Services;
        private readonly IConfiguration _Configuration;
        private LoggingSettings _JsonSettings;
        private LoggingConfiguration NlogNewConfiguration;
        private LogBuilder _LogBuilder;
        private MyUnitOfLog(IServiceCollection services, IConfiguration configuration)
        {
            _Services = services;
            _LogBuilder = new LogBuilder();
            _Configuration = configuration;
            NlogNewConfiguration = new LoggingConfiguration();
        }


        public static MyUnitOfLog CreateUnit(IServiceCollection services, IConfiguration configuration)
        {
            return new MyUnitOfLog(services, configuration);
        }

        public MyUnitOfLog UseJsonConfiguration(string KeySection)
        {
            _JsonSettings = JsonConfiguration.BindSettings(_Configuration, KeySection);
            _LogBuilder.JsonSettings();
            return this;
        }



        public MyUnitOfLog Configure(Action<LoggerOptions> loggerOptions)
        {
            //this.ConfigureLaunched = true;

            if (loggerOptions is null)
            {
                ArgumentNullException argumentNullException = new(nameof(Configure), "configureOptions cannot be null");
                throw argumentNullException;
            }

         


            var ConfigureOptions = new LoggerOptions();

           /* if (this.SettingsFromJson)
            {
                options = new LoggerOptions(_LoggersAgents, _JsonSettings!, SettingsFromJson);
            }
            */
            loggerOptions(ConfigureOptions);


            _LogBuilder.IsDefault = ConfigureOptions.IsDefault();


            /*this.DefaultPresets = options.IsDefault();

            if (!this.DefaultPresets)
            {
                _LoggersAgents = options.GetLoggersNames();
            }
            */


            return this;
        }
















        //se llama casi al final para constuir los loggers ocnes ta ocnfiguracion
        public MyUnitOfLog Build()
        {
            
            _Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            });

            if (_LogBuilder.Json()) 
            {
                if (_LogBuilder.Status())
                    _LogBuilder.SetLoggersFromJson(_JsonSettings.LoggingActive, _JsonSettings.Loggers.FirstOrDefault()!);
                else 
                    return this;
            }
            LoggingConfiguration nuevo =new LoggingConfiguration();


            LoggerOptionsManager loggerOptionsManager = new LoggerOptionsManager();

            loggerOptionsManager.SetDefaultLoggersConfiguration(_LogBuilder.GetLoggers(),  nuevo);

            _LogBuilder.SetloggingConfiguration(nuevo);

            return this;    
        }





























        public void BuildAA()
        {


          //  BuildTargetOptions();

            LogManager.Configuration = _LogBuilder.GetLoggingConfiguration();



            RegisterLoggins();

            _Services.AddLogging(logging => logging.AddNLog());


            return this;
        }


        //private void BuildTargetOptions()
        //{
        //    var Options = new TargetsActualConfigu();

        //    if (_LogBuilder.Json())
        //    {
        //        Options.ConsoleLog.IsActive = _LogBuilder.LoggingsSettings!.Loggers[0].Console.Active;
        //        Options.FileLog.IsActive = _LogBuilder.LoggingsSettings!.Loggers[0].File.Active;
        //        Options.SeqLog.IsActive = _LogBuilder.LoggingsSettings!.Loggers[0].Seq.Active;
        //    }

        //    //Options.ConsoleLog.Async = false;
        //    //Options.FileLog.Async = ActivationOptions.FileLog;
        //    //Options.SeqLog.Async = ActivationOptions.SeqLog;

        //    _LogBuilder.targetsActualConfigu = Options;
        //}

        private void RegisterLoggins()
        {
            _Services.AddScoped<IMyLogger, LoggerManager>(provider =>
            {
                var instance = new LoggerManager();

                if (_LogBuilder.Status())
                {
                    instance.Activate();
                    var Loggers = AddMyLoggers(provider.GetService<ILoggerFactory>()!);
                    instance.SetLoggers(Loggers);
                }
                return instance;
            });

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



}
