using MyLoggingUnit.Tools;
using NLog.Config;

namespace MyLoggingUnit.Core
{
    //public partial class TargetingOptionsConfiguration
    //{
    //    private LoggingConfiguration _TargetLoggerconfig;
    //    private Dictionary<LoggingTarget, string> _LoggersNames;
    //    private TargetsPrivateConfiguration _PrivateTargetConfig;
    //    public TargetingOptionsConfiguration()
    //    {
    //        _TargetLoggerconfig = new LoggingConfiguration();
    //        _LoggersNames = new Dictionary<LoggingTarget, string>();
    //        _PrivateTargetConfig = new TargetsPrivateConfiguration();
    //    }

    //    public TargetingOptionsConfiguration AddConsoleLogging(Action<ConsoleConfiguration> targetConfig)
    //    {
    //        var options = new ConsoleConfiguration();
    //        targetConfig(options);

    //        if (!_LoggersNames.Any()) throw new Exception("No loggers registered");

    //        if (options.IsActive)
    //        {
    //        //    _TargetLoggerconfig = _PrivateTargetConfig.AddCustomConsoleTarget(_TargetLoggerconfig, options.ConsoleTargetConfig!, _LoggersNames[LoggingTarget.Console]);
    //        }
    //        return this;
    //    }


    //    public TargetingOptionsConfiguration AddDefaultConsoleLogging(Action<IDefaultConfiguration> targetConfig)
    //    {
    //        IDefaultConfiguration options = new DefaultConfiguration();
    //        targetConfig(options);

    //        if (options.IsActive)
    //        {
    //            var name = ExtensionsMethods.GetKey(_LoggersNames, LoggingTarget.Console);
    //            _TargetLoggerconfig = _PrivateTargetConfig.AddDefaultColoredConsoleTarget(_TargetLoggerconfig, name);
    //        }
    //        return this;
    //    }

    //    public TargetingOptionsConfiguration AddFileLogging(Action<FileConfiguration> targetConfig)
    //    {
    //        var options = new FileConfiguration();
    //        targetConfig(options);

    //        if (!_LoggersNames.Any()) throw new Exception("No loggers registered");

    //        if (options.IsActive)
    //        {
    //        //    _TargetLoggerconfig = _PrivateTargetConfig.AddCustomFileTarget(_TargetLoggerconfig, options.FileTargetConfig!, _LoggersNames[LoggingTarget.File]);

    //        }
    //        return this;
    //    }



    //    public TargetingOptionsConfiguration AddDefaultFileLogging(Action<IDefaultConfiguration> targetConfig)
    //    {
    //        IDefaultConfiguration options = new DefaultConfiguration();
    //        targetConfig(options);

    //        if (!_LoggersNames.Any()) throw new Exception("No loggers registered");

    //        if (options.IsActive)
    //        {
    //            var name = ExtensionsMethods.GetKey(_LoggersNames, LoggingTarget.File);
    //            _TargetLoggerconfig = _PrivateTargetConfig.AddDefaultFileTarget(_TargetLoggerconfig, name);
    //            _TargetLoggerconfig = _PrivateTargetConfig.AddDefaultErrorFileTarget(_TargetLoggerconfig, name);
    //        }

    //        return this;
    //    }


    //    public TargetingOptionsConfiguration AddSeq(Action<SeqConfiguration> targetConfig)
    //    {
    //        var options = new SeqConfiguration();
    //        targetConfig(options);

    //        if (!_LoggersNames.Any()) throw new Exception("No loggers registered");

    //        CheckSeqValues(options);
    //       // options.SeqTargetConfig = _PrivateTargetConfig.BuildSeqTarget(options);

    //        _TargetLoggerconfig = _PrivateTargetConfig.AddSeqTarget(_TargetLoggerconfig, options.SeqTargetConfig, _LoggersNames[LoggingTarget.Seq]);

    //        return this;
    //    }

    //    public TargetingOptionsConfiguration AddDefaultSeq()
    //    {
    //        if (!_LoggersNames.Any()) throw new Exception("No loggers registered");

    //        var seq = _PrivateTargetConfig.BuildDefaultSeqTarget();

    //        var name = ExtensionsMethods.GetKey(_LoggersNames, LoggingTarget.Seq);
    //        _TargetLoggerconfig = _PrivateTargetConfig.AddSeqTarget(_TargetLoggerconfig, seq, name);

    //        return this;
    //    }

    //    public void SetDictionary(Dictionary<LoggingTarget, string> data)
    //    {
    //        _LoggersNames = data;
    //    }


    //    private static void CheckSeqValues(SeqConfiguration config)
    //    {
    //        if (string.IsNullOrEmpty(config.ServerUrl) || string.IsNullOrEmpty(config.ApiKey))
    //        {
    //            throw new ArgumentNullException(nameof(config));
    //        }
    //    }


    //    public LoggingConfiguration GetLogConfiguration()
    //    {
    //        return _TargetLoggerconfig;
    //    }
    //}



}
