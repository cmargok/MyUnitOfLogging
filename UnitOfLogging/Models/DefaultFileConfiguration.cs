using LogLevel = NLog.LogLevel;

namespace UnitOfLogging.Models
{
    public interface IDefaultConfiguration
    {
        public bool IsActive { get; set; }
    }
    public class DefaultConfiguration : IDefaultConfiguration
    {
        public bool IsActive { get; set; } = true;
    }













}