using NLog.Targets;
namespace MyLoggingUnit.Models.TargetsContracts
{
    public interface IConsoleConfig : ITargetConfiguration
    {
        public ColoredConsoleTarget? ConsoleTargetConfig { get; set; }
    }

}
