using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace UnitOfLogging.Logging
{
    public sealed class LoggingSettings
    {
        public bool LoggingActive { get; set; } = false;
        public List<LoggersInitialConfigs> Loggers { get; set; }

        public IEnumerable<string> GetActiveLoggers()
        {
            foreach (var logger in Loggers)
            {
                if (logger.Active)
                {
                    yield return logger.Name;
                }
            }
        }

 

        public class LoggersInitialConfigs
        {
            public bool Active { get; set; } = false;
            public string Name { get; set; } = string.Empty;

            [JsonConverter(typeof(JsonStringEnumConverter))]
            public LoggingTarget Target { get; set; } = LoggingTarget.None;
        }
    }

    public enum LoggingTarget
    {
        [field: Description("None")]
        None = 0,

        [field: Description("Console")]
        Console = 1,

        [field: Description("File")]
        File = 2,

        [field: Description("Seq")]
        Seq = 3,

        [field: Description("ElasticSearch")]
        ElasticSearch = 4,

        [field: Description("Database")]
        Database = 5,
    }


}
