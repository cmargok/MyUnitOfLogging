using NLog.Targets;
using NLog.Config;
using LogLevel = NLog.LogLevel;
using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using NLog.Fluent;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Options;
using UnitOfLogging.Models;
using UnitOfLogging.Models.Contracts;
using static System.Net.WebRequestMethods;
using System.Reflection;

namespace UnitOfLogging.Core.TargetsManagerismo
{
    public sealed class TargetsPrivateConfiguration
    {
        public IConsoleConfig? ConsoleConfiguration { get; set; }
        public IFileConfig? FileConfig { get; set; }
        public ISeqConfig? SeqConfig { get; set; }

        private string logDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

        public TargetsPrivateConfiguration()
        {
        }
        private string SetDirectoryPathEnding(string logFileName)
        {
            return Path.Combine(logDirectory, logFileName);
        }




        public LoggingConfiguration AddDefaultColoredConsoleTarget(LoggingConfiguration loggerconfig, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddDefaultColoredConsoleTarget));
            ConsoleConfiguration = new ConsoleConfiguration();
            ConsoleConfiguration.ConsoleTargetConfig = new ColoredConsoleTarget();
            ConsoleConfiguration.ConsoleTargetConfig.Name = ConsoleConfiguration.TargetName;
            ConsoleConfiguration.ConsoleTargetConfig.Layout = @"${longdate} ${uppercase:${level}} ${logger} ${message}";
            TargetingConfig.AddRule(loggerconfig, ConsoleConfiguration.TargetName, ConsoleConfiguration.TargetLogLevel, ConsoleConfiguration.ConsoleTargetConfig, LoggerName);

            return loggerconfig;

        }

        public LoggingConfiguration AddCustomConsoleTarget(LoggingConfiguration loggerconfig, ColoredConsoleTarget consoleTarget, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddCustomConsoleTarget));

            if (consoleTarget is null)
            {
                return AddDefaultColoredConsoleTarget(loggerconfig, LoggerName); ;
            }
            TargetingConfig.AddRule(loggerconfig, ConsoleConfiguration!.TargetName, ConsoleConfiguration.TargetLogLevel, consoleTarget, LoggerName);

            return loggerconfig;
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
            TargetingConfig.AddRule(loggerconfig, FileConfig!.TargetName, FileConfig.TargetLogLevel, FileConfig.FileTargetConfig, LoggerName);

            return loggerconfig;

        }

        public LoggingConfiguration AddDefaultErrorFileTarget(LoggingConfiguration loggerconfig, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddDefaultErrorFileTarget));

            FileConfig = null;
            FileConfig = new FileConfiguration();
            FileConfig.FileTargetConfig = new FileTarget();
            FileConfig.FileTargetConfig.FileName = SetDirectoryPathEnding("Log\\Error_Log_${shortdate}.txt");
            FileConfig.FileTargetConfig.Layout = "${longdate} ${uppercase:${level}} ${logger} ${message} ${newline}${exception:format=ToString} ${newline} ${stacktrace} "
                + "${newline}  ${aspnet-request-routeparameters}  ${newline}	${aspnet-request-url} ${newline} ${aspnet-response-statuscode}";
            FileConfig.FileTargetConfig.Name = "Errorlogfile";
            FileConfig.TargetName = "Errorlogfile";
            FileConfig.TargetLogLevel = LogLevel.Error;
            TargetingConfig.AddRule(loggerconfig, FileConfig!.TargetName, FileConfig.TargetLogLevel, FileConfig.FileTargetConfig!, LoggerName);

            return loggerconfig;
        }



        public LoggingConfiguration AddCustomFileTarget(LoggingConfiguration loggerconfig, FileTarget fileTarget, string LoggerName)
        {
            if (loggerconfig is null) throw new ArgumentNullException(nameof(AddCustomFileTarget));

            if (fileTarget is null)
            {
                var logConfig = AddDefaultFileTarget(loggerconfig, LoggerName);
                return AddDefaultErrorFileTarget(logConfig, LoggerName);
            }
            if (string.IsNullOrEmpty(fileTarget.FileName.ToString())) fileTarget.FileName = SetDirectoryPathEnding("${var:logDirectory}/Log_${shortdate}.txt");
            TargetingConfig.AddRule(loggerconfig, FileConfig!.TargetName, FileConfig.TargetLogLevel, fileTarget, LoggerName);

            return loggerconfig;
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

        public BufferingTargetWrapper BuildSeqTarget(SeqConfiguration options)
        {
            var seqTarget = new SeqTarget()
            {
                ServerUrl = options.ServerUrl,
                ApiKey = options.ApiKey ?? null,
            };

            if (options.Properties!.Any())
            {
                foreach (var item in options.Properties!)
                {
                    seqTarget.Properties.Add(new SeqPropertyItem
                    {
                        Name = item.Name,
                        As = item.As,
                        Value = item.Value,
                    });
                }
            }
            else if (options.UseDefaultProperties)
            {
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
            }

            if (options.UseDefaultBufferingProperties)
            {
                options.SeqTargetConfig!.BufferSize = 1024;
                options.SeqTargetConfig!.FlushTimeout = 2000;
                options.SeqTargetConfig!.SlidingTimeout = false;
            }

            options.SeqTargetConfig!.WrappedTarget = seqTarget;
            return options.SeqTargetConfig!;

        }


        public LoggingConfiguration AddSeqTarget(LoggingConfiguration loggerconfig, BufferingTargetWrapper buffering, string LoggerName)
        {

            if (buffering is null || loggerconfig is null) throw new ArgumentNullException(nameof(AddSeqTarget));
            TargetingConfig.AddRule(loggerconfig, SeqConfig!.TargetName, SeqConfig.TargetLogLevel, buffering, LoggerName);
            return loggerconfig;

        }



     




    }


    public static class TargetingConfig
    {
        public static LoggingConfiguration AddRule(this LoggingConfiguration loggerconfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            loggerconfig.AddTarget(TargetName, target);
            loggerconfig.LoggingRules.Add(new LoggingRule(LoggerName, level, target));
            return loggerconfig;
        }

        public static LoggingConfiguration AddRuleAsync(this LoggingConfiguration loggerconfig, string TargetName, LogLevel level, Target target, string LoggerName)
        {
            var asyncFileTarget = new AsyncTargetWrapper(target);
            loggerconfig.AddTarget(TargetName, asyncFileTarget);
            loggerconfig.LoggingRules.Add(new LoggingRule(LoggerName, level, asyncFileTarget));
            return loggerconfig;
        }

    }















}