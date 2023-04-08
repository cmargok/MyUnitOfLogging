using NLog.Targets;
using UnitOfLogging.Models.Contracts;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging.Models
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