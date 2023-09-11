using NLog.Layouts;
using LogLevel = NLog.LogLevel;
namespace MyLoggingUnit.Models
{
    public interface ITargetConfiguration
    {
        public bool IsActive { get; set; }
        public LogLevel TargetLogLevel { get; set; }
        public string TargetName { get; set; }
    }



}
