using MyLoggingUnit.Models.TargetsContracts;
using NLog.Targets;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Models.Targets
{
    public class ConsoleConfiguration : IConsoleConfig
    {
        public ColoredConsoleTarget? ConsoleTargetConfig { get; set; }
        public bool IsActive { get; set; } = true;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
        public string TargetName { get; set; } = "console";
        public bool DefaultConsoleLogSettings { get; set; } = true;
    }
}
