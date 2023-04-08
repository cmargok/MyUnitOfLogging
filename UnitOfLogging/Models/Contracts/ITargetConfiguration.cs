using NLog.Targets.Seq;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;

namespace UnitOfLogging
{
    public interface ITargetConfiguration
    {
        public bool IsActive { get; set; }
        public LogLevel TargetLogLevel { get; set; } 
        public string TargetName { get; set; } 
    }




}