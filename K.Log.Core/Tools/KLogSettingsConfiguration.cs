
using ahuevo;
using K.Logger.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace K.Logger.Core.Tools
{
    public static class KLogSettingsConfiguration
    {
        private static readonly string ErrorMessage = "Settings json section was no configured correctly";

        public static RabbitMQClientConfiguration GetKLogConfig(this IConfiguration config)
        {
            string KeySection = "KLogSettings";
            RabbitMQClientConfiguration Settings = config.GetSection(KeySection).Get<RabbitMQClientConfiguration>()!;

          //  Settings.ThrowIfNull(ErrorMessage);

            return Settings;
        }
    }
}
