using Microsoft.Extensions.Options;
using NLog.Config;
using UnitOfLogging.Core.TargetConf;
using UnitOfLogging.Models.ExplicitConfiguration;

namespace UnitOfLogging.Core
{
    public sealed class LoggerManagerOptions
    {
        private LoggingConfiguration _Loggerconfig;

        private Dictionary<LoggingTarget, string> _LoggersNamesDictionary;

        public LoggerManagerOptions(Dictionary<LoggingTarget, string> dictionary)
        {
            _LoggersNamesDictionary = dictionary;
            _Loggerconfig = new();
        }

        public LoggerManagerOptions AddTargets(Action<TargetingOptionsConfiguration> Targets = null!)
        {
            if (!_LoggersNamesDictionary.Any()) throw new Exception("No loggers registered");

            _Loggerconfig = new LoggingConfiguration();
            if (Targets != null)
            {
                var options = new TargetingOptionsConfiguration();
                options.SetDictionary(_LoggersNamesDictionary);
                Targets(options);               
                _Loggerconfig = options.GetLogConfiguration();
            }     

            return this;
        }              

        public LoggingConfiguration BuildConfig()
        {
            return _Loggerconfig;
        }
    }
}
