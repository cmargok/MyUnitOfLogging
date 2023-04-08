using NLog.Targets;

namespace UnitOfLogging.Models.Contracts
{
    public interface IFileConfig : ITargetConfiguration
    {
        public FileTarget? FileTargetConfig { get; set; }
    }














}