using NLog.Targets.Seq;
using NLog.Targets.Wrappers;

namespace UnitOfLogging
{
    public interface ISeqConfig : ITargetConfiguration
    {
        public BufferingTargetWrapper? SeqTargetConfig { get; set; }
        public List<SeqPropertyItem> Properties { get; set; }
        public bool UseDefaultProperties { get; set; }
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
        public bool UseDefaultBufferingProperties { get; set; }
    }












}