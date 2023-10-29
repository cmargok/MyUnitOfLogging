using Microsoft.Extensions.Logging;

namespace K.Loggger.Client.Logger
{
    internal class MyLogger : IMyLogger
    {
        private readonly ILogger<MyLogger> _logger;

        public MyLogger(ILogger<MyLogger> logger)
        {
            _logger = logger;
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
