using Nlog.RabbitMQ.Target;
namespace MyLoggingUnit.Models.TargetsContracts
{
    public interface IRabbitMQTargetConfig : ITargetConfiguration
    {
        public RabbitMQTarget RabbitMQTarget { get; set; }
        public string AppName { get; set; } 

    }
}
