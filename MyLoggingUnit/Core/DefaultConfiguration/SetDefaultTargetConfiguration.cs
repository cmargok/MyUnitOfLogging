using Microsoft.Extensions.Logging;
using MyLoggingUnit.Models.Targets;
using MyLoggingUnit.Models.TargetsContracts;
using Nlog.RabbitMQ.Target;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using System.Reflection;
using LogLevel = NLog.LogLevel;



namespace MyLoggingUnit.Core.DefaultConfiguration
{


    public sealed class SetDefaultTargetConfiguration
    {
        public IConsoleConfig? ConsoleConfiguration { get; set; }
        public IFileConfig? FileConfig { get; set; }
        public ISeqConfig? SeqConfig { get; set; }

        public IRabbitMQTargetConfig? RabbitMQConfig { get; set; }


        private string logDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

        public SetDefaultTargetConfiguration()
        {
        }
        private string SetDirectoryPathEnding(string logFileName)
        {
            return Path.Combine(logDirectory, logFileName);
        }

        public LoggingConfiguration AddDefaultRabbitMQ(LoggingConfiguration loggerConfig, string LoggerName)
        {
            if (loggerConfig is null) throw new ArgumentNullException(nameof(AddDefaultRabbitMQ));

            RabbitMQConfig = new RabbitMQTargetConfig();

            RabbitMQConfig.RabbitMQTarget = new RabbitMQTarget();
            RabbitMQConfig.RabbitMQTarget.Name = RabbitMQConfig.TargetName;
            RabbitMQConfig.RabbitMQTarget.AppId = RabbitMQConfig.AppName;
            RabbitMQConfig.RabbitMQTarget.UseJSON = true;
            RabbitMQConfig.RabbitMQTarget.Layout = "${longdate} ${uppercase:${level}} ${logger} ${message} ${newline}${exception:format=ToString} ${newline} ${stacktrace} "
                + "${newline}  ${aspnet-request-routeparameters}  ${newline}	${aspnet-request-url} ${newline} ${aspnet-response-statuscode}";

            loggerConfig.AddRule(RabbitMQConfig.TargetName, RabbitMQConfig.TargetLogLevel, RabbitMQConfig.RabbitMQTarget, LoggerName);

            return loggerConfig;

        }

        public LoggingConfiguration AddDefaultColoredConsoleTarget(LoggingConfiguration loggerConfig, string LoggerName)
        {
            if (loggerConfig is null) throw new ArgumentNullException(nameof(AddDefaultColoredConsoleTarget));

            ConsoleConfiguration = new ConsoleConfiguration();
            ConsoleConfiguration.ConsoleTargetConfig = new ColoredConsoleTarget();
            ConsoleConfiguration.ConsoleTargetConfig.Name = ConsoleConfiguration.TargetName;
            ConsoleConfiguration.ConsoleTargetConfig.Layout = @"${longdate} ${uppercase:${level}} ${logger} ${message}";
            loggerConfig.AddRule(ConsoleConfiguration.TargetName, ConsoleConfiguration.TargetLogLevel, ConsoleConfiguration.ConsoleTargetConfig, LoggerName);

            return loggerConfig;

        }
        public LoggingConfiguration AddDefaultFileTarget(LoggingConfiguration loggerconfig, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddDefaultFileTarget));

            FileConfig = null;
            FileConfig = new FileConfiguration();
            FileConfig.FileTargetConfig = new FileTarget();
            FileConfig.FileTargetConfig.FileName = SetDirectoryPathEnding("Log\\Log_${shortdate}.txt");
            FileConfig.FileTargetConfig.Layout = "${longdate} ${uppercase:${level}} ${logger} => ${message}";
            FileConfig.FileTargetConfig.Name = "Flowlogfile";
            loggerconfig.AddRule(FileConfig!.TargetName, FileConfig.TargetLogLevel, FileConfig.FileTargetConfig, LoggerName);

            return loggerconfig;

        }
        public LoggingConfiguration AddDefaultErrorFileTarget(LoggingConfiguration loggerConfig, string LoggerName)
        {
            if (loggerConfig is null) throw new ArgumentNullException(nameof(AddDefaultErrorFileTarget));

            FileConfig = null;
            FileConfig = new FileConfiguration();
            FileConfig.FileTargetConfig = new FileTarget();
            FileConfig.FileTargetConfig.FileName = SetDirectoryPathEnding("Log\\Error_Log_${shortdate}.txt");
            FileConfig.FileTargetConfig.Layout = "${longdate} ${uppercase:${level}} ${logger} ${message} ${newline}${exception:format=ToString} ${newline} ${stacktrace} "
                + "${newline}  ${aspnet-request-routeparameters}  ${newline}	${aspnet-request-url} ${newline} ${aspnet-response-statuscode}";
            FileConfig.FileTargetConfig.Name = "Errorlogfile";
            FileConfig.TargetName = "Errorlogfile";
            FileConfig.TargetLogLevel = LogLevel.Error;
            loggerConfig.AddRule(FileConfig!.TargetName, FileConfig.TargetLogLevel, FileConfig.FileTargetConfig!, LoggerName);

            return loggerConfig;
        }


        public BufferingTargetWrapper BuildDefaultSeqTarget()
        {
            SeqConfig = new SeqConfiguration();
            var seqTarget = new SeqTarget()
            {
                ServerUrl = "http://localhost:5341",
                ApiKey = "",
            };

            seqTarget.Properties.Add(new SeqPropertyItem
            {
                Name = "Application",
                As = "Application",
                Value = "MinimalApi",
            });

            seqTarget.Properties.Add(new SeqPropertyItem
            {
                Name = "Environment",
                As = "Environment",
                Value = "Development",
            });
            BufferingTargetWrapper bufferingTarget = new();

            bufferingTarget!.BufferSize = 1024;
            bufferingTarget!.FlushTimeout = 2000;
            bufferingTarget!.SlidingTimeout = false;
            bufferingTarget!.WrappedTarget = seqTarget;

            return bufferingTarget!;

        }



        public LoggingConfiguration AddSeqTarget(LoggingConfiguration loggerconfig, BufferingTargetWrapper buffering, string LoggerName)
        {

            if (buffering is null || loggerconfig is null) throw new ArgumentNullException(nameof(AddSeqTarget));
            loggerconfig.AddRule(SeqConfig!.TargetName, SeqConfig.TargetLogLevel, buffering, LoggerName);
            return loggerconfig;

        }



        /*
        public LoggingConfiguration AddCustomConsoleTarget(LoggingConfiguration loggerconfig, ColoredConsoleTarget consoleTarget, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddCustomConsoleTarget));

            if (consoleTarget is null)
            {
                return AddDefaultColoredConsoleTarget(loggerconfig, LoggerName); ;
            }
            TargetingConfig.AddRule(loggerconfig, ConsoleConfiguration!.TargetName, ConsoleConfiguration.TargetLogLevel, consoleTarget, LoggerName);

            return loggerconfig;
        }*/

        //public LoggingConfiguration AddCustomFileTarget(LoggingConfiguration loggerconfig, FileTarget fileTarget, string LoggerName)
        //{
        //    if (loggerconfig is null) throw new ArgumentNullException(nameof(AddCustomFileTarget));

        //    if (fileTarget is null)
        //    {
        //        var logConfig = AddDefaultFileTarget(loggerconfig, LoggerName);
        //        return AddDefaultErrorFileTarget(logConfig, LoggerName);
        //    }
        //    if (string.IsNullOrEmpty(fileTarget.FileName.ToString())) fileTarget.FileName = SetDirectoryPathEnding("${var:logDirectory}/Log_${shortdate}.txt");
        //    TargetingConfig.AddRule(loggerconfig, FileConfig!.TargetName, FileConfig.TargetLogLevel, fileTarget, LoggerName);

        //    return loggerconfig;
        //}


        //public BufferingTargetWrapper BuildSeqTarget(SeqConfiguration options)
        //{
        //    var seqTarget = new SeqTarget()
        //    {
        //        ServerUrl = options.ServerUrl,
        //        ApiKey = options.ApiKey ?? null,
        //    };

        //    if (options.Properties!.Any())
        //    {
        //        foreach (var item in options.Properties!)
        //        {
        //            seqTarget.Properties.Add(new SeqPropertyItem
        //            {
        //                Name = item.Name,
        //                As = item.As,
        //                Value = item.Value,
        //            });
        //        }
        //    }
        //    else if (options.UseDefaultProperties)
        //    {
        //        seqTarget.Properties.Add(new SeqPropertyItem
        //        {
        //            Name = "Application",
        //            As = "Application",
        //            Value = "MinimalApi",
        //        });

        //        seqTarget.Properties.Add(new SeqPropertyItem
        //        {
        //            Name = "Environment",
        //            As = "Environment",
        //            Value = "Development",
        //        });
        //    }

        //    if (options.UseDefaultBufferingProperties)
        //    {
        //        options.SeqTargetConfig!.BufferSize = 1024;
        //        options.SeqTargetConfig!.FlushTimeout = 2000;
        //        options.SeqTargetConfig!.SlidingTimeout = false;
        //    }

        //    options.SeqTargetConfig!.WrappedTarget = seqTarget;
        //    return options.SeqTargetConfig!;

        //}



    }

}
