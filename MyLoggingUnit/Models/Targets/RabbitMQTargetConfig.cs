using MyLoggingUnit.Models.TargetsContracts;
using Nlog.RabbitMQ.Target;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Models.Targets
{
    public class RabbitMQTargetConfig : IRabbitMQTargetConfig
    {
        public bool IsActive { get; set; } = false;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Error;
        public string TargetName { get; set; } = "rabbitMQ";
        public RabbitMQTarget RabbitMQTarget { get; set; } = new RabbitMQTarget();

        public string AppName { get; set; } = string.Empty;
    }
}
