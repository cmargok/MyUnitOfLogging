using NLog.Targets;

namespace UnitOfLogging.Models.Contracts
{
    public interface IConsoleConfig : ITargetConfiguration
    {
        public ColoredConsoleTarget? ConsoleTargetConfig { get; set; }
    }


















}