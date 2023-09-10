namespace MyLoggingUnit
{
    public class LoggersSet
    {
        public LoggersConfigs Console { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Console };
        public LoggersConfigs File { get; set; } = new LoggersConfigs() { Target = LoggingTarget.File };
        public LoggersConfigs Seq { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Seq };

        public List<LoggersConfigs> GetLoggerList() => new List<LoggersConfigs>() { Console, File, Seq };


    }
}
