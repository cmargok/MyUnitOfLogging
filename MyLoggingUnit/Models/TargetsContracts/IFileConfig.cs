using NLog.Targets;
namespace MyLoggingUnit.Models.TargetsContracts
{
    public interface IFileConfig : ITargetConfiguration
    {
        public FileTarget? FileTargetConfig { get; set; }
    }

}
