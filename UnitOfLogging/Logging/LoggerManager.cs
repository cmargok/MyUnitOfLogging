
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using UnitOfLogging.Models.ExplicitConfiguration;

namespace UnitOfLogging.Logging
{
   /* public interface ILoggerManager : IMyLogger
    {
        public void AddDefaultLoggers(ILoggerFactory loggerFactory, Dictionary<LoggingTarget, string> loggersNames);
    }*/
    public sealed class LoggerManager : IMyLogger
    {

        private List<IMyLogger> _apiLoggers;
      //  private readonly LoggingSettings _loggingSettings;
        //private readonly Dictionary<LoggingTarget, string> _loggers;
        private bool IsLoggingActive = false;

        public LoggerManager(ILoggerFactory loggerFactory)
        {
            _apiLoggers = new List<IMyLogger>();
           // _loggingSettings = settings;
            //_loggers = new();
           // InitializeLoggers(loggerFactory);
        }
        public LoggerManager()
        {
            _apiLoggers = new List<IMyLogger>();
        }
        /*private void InitializeLoggers(ILoggerFactory loggerFactory)
        {
            if (_loggingSettings.LoggingActive)
            {
                foreach (var logger in _loggingSettings.GetActiveLoggers())
                {

                    _apiLoggers.Add(new MyLogger(loggerFactory, logger.Name));
                    _loggers.Add(logger.Target, logger.Name);
                }
            }
        }*/

        public void SetLoggers(List<IMyLogger>  value) => _apiLoggers = value;
      
        public void Activate() => IsLoggingActive = true;



        public void LoggingInformation(string message)
        {
            if (IsLoggingActive)
            {
                //foreach(var logger in _apiLoggers)
                //{
                //    logger.LoggingInformation(message);
                //}
                _apiLoggers.ForEach(logger => logger.LoggingInformation(message));
            }
        }

        public void LoggingWarning(string message)
        {
            if (IsLoggingActive)
            {
                _apiLoggers.ForEach(logger => logger.LoggingWarning(message));
               
            }
        }

        public void LoggingError(Exception ex, string message)
        {
            if (IsLoggingActive)
            {
                _apiLoggers.ForEach(logger => logger.LoggingError(ex,message));
              
            }
        }




    }

}
