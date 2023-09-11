namespace MyLoggingUnit.Logger
{
    public interface IMyLogger
    {
        public void LoggingWarning(string message);
        public void LoggingInformation(string message);
        public void LoggingError(Exception ex, string message);
    }


}
