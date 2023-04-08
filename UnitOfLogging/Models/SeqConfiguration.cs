using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging.Models
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