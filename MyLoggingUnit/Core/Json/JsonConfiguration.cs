using Microsoft.Extensions.Configuration;
using MyLoggingUnit.BuilderAndSettings;
using MyLoggingUnit.Tools;
namespace MyLoggingUnit.Core.Json
{
    public static class JsonConfiguration
    {
        private static readonly string ErrorMessage = "LogSettings json section was no configured correctly";

        public static LoggingSettings BindSettings(IConfiguration config, string KeySection)
        {
            LoggingSettings Settings = config.GetSection(KeySection)
                .Get<LoggingSettings>()!;

            Settings.ThrowIfNull(ErrorMessage);

            if (Settings.Loggers.Count == 0) throw new Exception(ErrorMessage);

            return Settings;
        }
    }
}
