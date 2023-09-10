namespace MyLoggingUnit
{
    public sealed class LoggingSettings
    {
        public bool LoggingActive { get; set; } = false;
        public List<LoggersSet> Loggers { get; set; } = new();
     

         //public IEnumerable<LoggersConfigs> GetActiveLoggers()
         //{

         //    foreach (var logger in Loggers[0].GetLoggerList())
         //    {
         //        if (logger.Active)
         //        {
         //            yield return logger;
         //        }
         //    }
         //    yield break;
         //}
         


       
    }
}
