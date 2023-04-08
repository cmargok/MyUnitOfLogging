using NLog.Targets;
using UnitOfLogging.Models.Contracts;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging.Models
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