using MyLoggingUnit.Models.TargetsContracts;
using NLog.Targets;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Models.Targets
{
    public class FileConfiguration : IFileConfig
    {
        public FileTarget? FileTargetConfig { get; set; }
        public bool IsActive { get; set; } = true;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
        public string TargetName { get; set; } = "Flowlogfile";
        public bool DefaultFileLogSettings { get; set; } = true;

    }

}
