using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using UnitOfLogging.Models.ExplicitConfiguration;

namespace UnitOfLogging.Models.ExplicitConfiguration
{
    public sealed class LoggingSettings
    {
        public bool LoggingActive { get; set; } = false;
        public List<LoggersSet> Loggers { get; set; } = new();
    
    
        public IEnumerable<LoggersConfigs> GetActiveLoggers()
        {

            foreach (var logger in Loggers[0].GetLoggerList())
            {                
                if (logger.Active)
                {
                    yield return logger;
                }
            }
            yield break;           
        }

        public class LoggersSet
        {
            public LoggersConfigs Console { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Console };
            public LoggersConfigs File { get; set; } = new LoggersConfigs() { Target = LoggingTarget.File };
            public LoggersConfigs Seq { get; set; } = new LoggersConfigs() { Target = LoggingTarget.Seq };

            public List<LoggersConfigs> GetLoggerList() => new List<LoggersConfigs>() { Console, File, Seq };
        }


        public class LoggersConfigs
        {
            [Required]
            public bool Active { get; set; } = false;

            [Required]
            public string Name { get; set; } = string.Empty;

            public LoggingTarget Target { get; set; } = LoggingTarget.None;

            public bool Async { get; set; } = false;
        }
    }


}
