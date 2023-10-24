using Microsoft.Extensions.Logging;
using MyLoggingUnit.Models.TargetsContracts;
using NLog;
using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Models.Targets
{


    public class SeqConfiguration : ISeqConfig
    {
        public BufferingTargetWrapper? SeqTargetConfig { get; set; }
        public bool IsActive { get; set; } = true;
        public LogLevel TargetLogLevel { get; set; } = LogLevel.Info;
        public string TargetName { get; set; } = "seqContainer";
        public List<SeqPropertyItem> Properties { get; set; } = new List<SeqPropertyItem>() 
        {  
            new SeqPropertyItem
                {
                    Name = "Application",
                    As = "Application",
                    Value = "MyApi",
                },

              new SeqPropertyItem
                {
                    Name = "Environment",
                    As = "Environment",
                    Value = "Development",
                }
        };
        public bool UseDefaultProperties { get; set; } = false;
        public string ServerUrl { get; set; } = "http://localhost:5341";
        public string ApiKey { get; set; } = string.Empty;
        public bool UseDefaultBufferingProperties { get; set; } = false;
    }


}
