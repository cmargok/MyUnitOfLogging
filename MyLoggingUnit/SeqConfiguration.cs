using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit
{

    public partial class TargetingOptionsConfiguration
    {
        public class SeqConfiguration : ISeqConfig
        {
            public BufferingTargetWrapper? SeqTargetConfig { get; set; }
            public bool IsActive { get; set; } = true;
            public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
            public string TargetName { get; set; } = "seqContainer";
            public List<SeqPropertyItem>? Properties { get; set; }
            public bool UseDefaultProperties { get; set; } = false;
            public string ServerUrl { get; set; } = String.Empty;
            public string ApiKey { get; set; } = String.Empty;
            public bool UseDefaultBufferingProperties { get; set; } = false;
        }
    }

    public interface ISeqConfig : ITargetConfiguration
    {
        public BufferingTargetWrapper? SeqTargetConfig { get; set; }
        public List<SeqPropertyItem> Properties { get; set; }
        public bool UseDefaultProperties { get; set; }
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
        public bool UseDefaultBufferingProperties { get; set; }
    }
    public interface ITargetConfiguration
    {
        public bool IsActive { get; set; }
        public LogLevel TargetLogLevel { get; set; }
        public string TargetName { get; set; }
    }

    public interface IDefaultConfiguration
    {
        public bool IsActive { get; set; }
    }
    public class DefaultConfiguration : IDefaultConfiguration
    {
        public bool IsActive { get; set; } = true;
    }
    public class ConsoleConfiguration : IConsoleConfig
    {
        public ColoredConsoleTarget? ConsoleTargetConfig { get; set; }
        public bool IsActive { get; set; } = true;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
        public string TargetName { get; set; } = "console";
        public bool DefaultConsoleLogSettings { get; set; } = true;
    }

    public interface IConsoleConfig : ITargetConfiguration
    {
        public ColoredConsoleTarget? ConsoleTargetConfig { get; set; }
    }
    public class FileConfiguration : IFileConfig
    {
        public FileTarget? FileTargetConfig { get; set; }
        public bool IsActive { get; set; } = true;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
        public string TargetName { get; set; } = "Flowlogfile";
        public bool DefaultFileLogSettings { get; set; } = true;

    }
    public interface IFileConfig : ITargetConfiguration
    {
        public FileTarget? FileTargetConfig { get; set; }
    }

}
