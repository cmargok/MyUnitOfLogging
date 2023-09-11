using MyLoggingUnit.Models.Entities;

namespace MyLoggingUnit.BuilderAndSettings
{
    public sealed class LoggingSettings
    {
        public bool LoggingActive { get; set; } = false;
        public List<LoggersSet> Loggers { get; set; } = new();






        
    }
}
