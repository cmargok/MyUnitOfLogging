using MyLoggingUnit.Tools.Enums;

namespace MyLoggingUnit.Models.Entities
{
    public class LoggersSet
    {
        public LoggersConfigs Console { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Console };
        public LoggersConfigs File { get; set; } = new LoggersConfigs() { Target = LoggingTarget.File };
        public LoggersConfigs ErrorFile { get; set; } = new LoggersConfigs() { Target = LoggingTarget.ErrorFile };
        public LoggersConfigs Seq { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Seq };
        public LoggersConfigs RabbitMQ { get; set; } = new LoggersConfigs() { Target = LoggingTarget.RabbitMQ };

        public List<LoggersConfigs> GetLoggerList() => new List<LoggersConfigs>() { Console, File, ErrorFile, Seq, RabbitMQ };


    }
}
