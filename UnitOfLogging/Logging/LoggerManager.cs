
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnitOfLogging.Models.ExplicitConfiguration;

namespace UnitOfLogging.Logging
{
   /* public interface ILoggerManager : IMyLogger
    {
        public void AddDefaultLoggers(ILoggerFactory loggerFactory, Dictionary<LoggingTarget, string> loggersNames);
    }*/
    public sealed class LoggerManager : IMyLogger
    {

        private readonly List<IMyLogger> _apiLoggers;
        private readonly LoggingSettings _loggingSettings;
        private readonly Dictionary<LoggingTarget, string> _loggers;


        public LoggerManager(ILoggerFactory loggerFactory, LoggingSettings settings)
        {
            _apiLoggers = new List<IMyLogger>();
            _loggingSettings = settings;
            _loggers = new();
            InitializeLoggers(loggerFactory);
        }

        private void InitializeLoggers(ILoggerFactory loggerFactory)
        {
            if (_loggingSettings.LoggingActive)
            {
                foreach (var logger in _loggingSettings.GetActiveLoggers())
                {

                    _apiLoggers.Add(new MyLogger(loggerFactory, logger.Name));
                    _loggers.Add(logger.Target, logger.Name);
                }
            }

        }

        public void AddDefaultLoggers(ILoggerFactory loggerFactory, Dictionary<LoggingTarget, string> loggersNames)
        {
            if (_loggingSettings.LoggingActive)
            {

                foreach (var logger in loggersNames)
                {
                    if (_loggers.TryAdd(logger.Key, logger.Value)) {

                        _apiLoggers.Add(new MyLogger(loggerFactory, logger.Value)); 
                    }

                }
            }            

        }



        public void LoggingInformation(string message)
        {
            if (_loggingSettings.LoggingActive)
            {
                foreach (var logger in _apiLoggers)
                {
                    logger.LoggingInformation(message);
                }
            }
        }

        public void LoggingWarning(string message)
        {
            if (_loggingSettings.LoggingActive)
            {
                foreach (var logger in _apiLoggers)
                {
                    logger.LoggingWarning(message);
                }
            }
        }

        public void LoggingError(Exception ex, string message)
        {
            if (_loggingSettings.LoggingActive)
            {
                foreach (var logger in _apiLoggers)
                {
                    logger.LoggingError(ex, message);
                }
            }
        }




    }

}
