using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace UnitOfLogging.Models.ExplicitConfiguration
{
    public sealed class LoggingSettings
    {
        public bool LoggingActive { get; set; } = false;
        public List<LoggersInitialConfigs>? Loggers { get; set; }

        public IEnumerable<LoggersInitialConfigs> GetActiveLoggers()
        {
            if(LoggingActive && Loggers is not null)
            {
                foreach (var logger in Loggers!)
                {
                    if (logger.Active)
                    {
                        yield return logger;
                    }
                }
            }
            yield break;
           
        }



        public class LoggersInitialConfigs
        {
            [Required]
            public bool Active { get; set; } = false;

            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public LoggingTarget Target { get; set; } = LoggingTarget.None;
        }
    }


}
