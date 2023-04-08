using NLog.Filters;
using NLog.Internal;
using NLog.Layouts;

namespace UnitOfLogging.Logging
{
    public interface IApiLogger
    {
        public void LoggingWarning(string message);
        public void LoggingInformation(string message);
        public void LoggingError(Exception ex, string message);
    }
}