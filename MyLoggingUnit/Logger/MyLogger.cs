using Microsoft.Extensions.Logging;

namespace MyLoggingUnit.Logger
{
    internal class MyLogger : IMyLogger
    {
        private readonly ILogger _logger;

        internal MyLogger(ILoggerFactory loggerFactory, string NameLogger)
        {
            _logger = loggerFactory.CreateLogger(NameLogger);
        }

        public void LoggingInformation(string message)
        {

            _logger.LogInformation(message);
        }

        public void LoggingWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LoggingError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }


}
