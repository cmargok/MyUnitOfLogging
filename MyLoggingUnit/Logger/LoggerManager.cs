using Microsoft.Extensions.Logging;

namespace MyLoggingUnit.Logger
{
    public sealed class LoggerManager : IMyLogger
    {
        private bool IsLoggingActive = false;
        private List<IMyLogger> _apiLoggers;

        public void Activate() => IsLoggingActive = true;

        public LoggerManager()
        {
            _apiLoggers = new List<IMyLogger>();
        }




        //  private readonly LoggingSettings _loggingSettings;
        //private readonly Dictionary<LoggingTarget, string> _loggers;


        //public LoggerManager(ILoggerFactory loggerFactory)
        //{
        //    _apiLoggers = new List<IMyLogger>();
        //    // _loggingSettings = settings;
        //    //_loggers = new();
        //    // InitializeLoggers(loggerFactory);
        //}

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

        public void SetLoggers(List<IMyLogger> value) => _apiLoggers = value;





        public void LoggingInformation(string message)
        {
            if (IsLoggingActive)
            {
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
                _apiLoggers.ForEach(logger => logger.LoggingError(ex, message));

            }
        }




    }

}
