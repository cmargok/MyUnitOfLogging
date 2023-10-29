

using Microsoft.Extensions.Configuration;

namespace K.Logger.Core.Tools
{
    public static class KLogSettingsConfiguration
    {
        private static readonly string ErrorMessage = "Settings json section was no configured correctly";

        public static EventClientConfiguration GetKLogConfig(this IConfiguration config)
        {
            string KeySection = "KLogSettings";
            var hko = config.GetSection(KeySection);
            EventClientConfiguration Settings = config.GetSection(KeySection).Get<EventClientConfiguration>()!;

          //  Settings.ThrowIfNull(ErrorMessage);

            return Settings;
        }
    }
       public class EventClientConfiguration
    {
        public IList<string> Hostnames { get; } = new List<string>();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public RabbitMQDeliveryMode DeliveryMode { get; set; } = RabbitMQDeliveryMode.NonDurable;
        public string RouteKey { get; set; } = string.Empty;
        public int Port { get; set; }
        public string ApiName { get; set; } = string.Empty;

        public EventClientConfiguration From(EventClientConfiguration config)
        {
            Username = config.Username;
            Password = config.Password;
            Exchange = config.Exchange;
            ExchangeType = config.ExchangeType;
            DeliveryMode = config.DeliveryMode;
            RouteKey = config.RouteKey;
            Port = config.Port;
            ApiName = config.ApiName;
            foreach (string hostName in config.Hostnames)
            {
                Hostnames.Add(hostName);
            }
            return this;
        }
    }
    public enum RabbitMQDeliveryMode : byte
    {
        NonDurable = 1,
        Durable = 2
    }
}
