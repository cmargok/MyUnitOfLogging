using Microsoft.Extensions.DependencyInjection;
using K.Logger.Core.Configuration;
using Microsoft.Extensions.Configuration;
using ahuevo;
using Serilog.Formatting.Json;
using Serilog;
using K.Loggger.Client.Logger;
using Serilog.Configuration;
using RabbitMQ.Client;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System.Text;
using Newtonsoft.Json;



namespace K.Logger.Client.Client
{
    public class KLog
    {
        private readonly IServiceCollection _services;

        private RabbitMQClientConfiguration _eventSettings;


        private KLog(IServiceCollection services)
        {
            _services = services;
            _eventSettings = new RabbitMQClientConfiguration();
        }

        public static KLog CreateClient(IServiceCollection services)
        {
            return new KLog(services);
        }

        public KLog AddSettings(RabbitMQClientConfiguration eventSettings)
        {
            _eventSettings = eventSettings;
            return this;
        }


        public KLog RegisterKLog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
            {
                clientConfiguration.From(_eventSettings!);
                sinkConfiguration.TextFormatter = new JsonFormatter();
                sinkConfiguration.RestrictedToMinimumLevel = LogEventLevel.Warning;
            }).CreateLogger();

            _services.AddSingleton<IMyLogger, MyLogger>();
            return this;
        }
     
    }
}


namespace Kbusbus
{
    public static class loggerExtender
    {
        private const int DefaultBatchPostingLimit = 50;
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);
        public static LoggerConfiguration ADDmikuka(this LoggerSinkConfiguration loggerConfiguration, Action<EventClientConfiguration, RabbitMQSinkConfiguration> configure)
        {
            EventClientConfiguration clientConfiguration = new EventClientConfiguration();
            RabbitMQSinkConfiguration sinkConfiguration = new RabbitMQSinkConfiguration();
            configure(clientConfiguration, sinkConfiguration);

            ValidateSettings(loggerConfiguration, clientConfiguration, sinkConfiguration);

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = sinkConfiguration.BatchPostingLimit,
                Period = sinkConfiguration.Period,
                EagerlyEmitFirstEvent = true,
                QueueLimit = 50
            };

            var RabbitMQSink = new PeriodicBatchingSink(new RabbitMQBatchSink(clientConfiguration), batchingOptions);
            return loggerConfiguration.Sink(RabbitMQSink, sinkConfiguration.RestrictedToMinimumLevel);

          

        }

        private static void ValidateSettings(LoggerSinkConfiguration loggerConfiguration, EventClientConfiguration clientConfiguration, RabbitMQSinkConfiguration sinkConfiguration) 
        {
            // guards
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (clientConfiguration.Hostnames.Count == 0) throw new ArgumentException("hostnames cannot be empty, specify at least one hostname", "hostnames");
            if (string.IsNullOrEmpty(clientConfiguration.Username)) throw new ArgumentException("username cannot be 'null' or and empty string.");
            if (clientConfiguration.Password == null) throw new ArgumentException("password cannot be 'null'. Specify an empty string if password is empty.");
            if (clientConfiguration.Port <= 0 || clientConfiguration.Port > 65535) throw new ArgumentOutOfRangeException("port", "port must be in a valid range (1 and 65535)");

            sinkConfiguration.BatchPostingLimit = (sinkConfiguration.BatchPostingLimit == default(int)) ? DefaultBatchPostingLimit : sinkConfiguration.BatchPostingLimit;
            sinkConfiguration.Period = (sinkConfiguration.Period == default(TimeSpan)) ? DefaultPeriod : sinkConfiguration.Period;
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
            foreach (string hostName in config.Hostnames)
            {
                Hostnames.Add(hostName);
            }
            return this;
        }
    }
    public class RabbitMQSinkConfiguration
    {
        public int BatchPostingLimit { get; set; }
        public TimeSpan Period { get; set; }
        public ITextFormatter TextFormatter { get; set; } = new JsonFormatter();
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
    }

    public class EventTo
    {
        public string ApiLog { get; set; } = string.Empty; 

        public string ApiLogFrom { get; set; }
        public EventTo(string apiLogFrom)
        {
            ApiLogFrom = apiLogFrom;
        }

       
    }

 
    public class RabbitMQBatchSink : IBatchedLogEventSink
    {
        private readonly EventClientConfiguration _configuration;
        private readonly ITextFormatter _formatter;
        private EventTo _eventTo;
        public RabbitMQBatchSink(EventClientConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            using var client = new RabbitMQClient(_configuration);

            foreach (var logEvent in batch)
            {
                var stringWriter = new StringWriter();

                _formatter.Format(logEvent, stringWriter);

                _eventTo.ApiLog = stringWriter.ToString();

                await client.PublishAsync(_eventTo);
            }
        }
        public async Task OnEmptyBatchAsync()
        {
           
          
        }
    }




    public class RabbitMQClient : IDisposable
    {
        // synchronization locks
        private const int MaxChannelCount = 64;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim[] _modelLocks = new SemaphoreSlim[MaxChannelCount];
        private readonly CancellationTokenSource _closeTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _closeToken;
        private int _currentModelIndex = -1;

        // configuration member
        private readonly EventClientConfiguration _config;
        private readonly PublicationAddress _publicationAddress;

        // endpoint members
        private readonly IConnectionFactory _connectionFactory;
        private readonly IModel[] _models = new IModel[MaxChannelCount];
        private readonly IBasicProperties[] _properties = new IBasicProperties[MaxChannelCount];
        private volatile IConnection _connection;

         public RabbitMQClient(EventClientConfiguration configuration)
        {
            _closeToken = _closeTokenSource.Token;

            for (var i = 0; i < MaxChannelCount; i++)
            {
                _modelLocks[i] = new SemaphoreSlim(1, 1);
            }

            // load configuration
            _config = configuration;
            
            _publicationAddress = new PublicationAddress(_config.ExchangeType, _config.Exchange, _config.RouteKey);

            // initialize
            _connectionFactory = SetConnectionFactory(configuration);
        }

        private IConnectionFactory SetConnectionFactory(EventClientConfiguration configuration)
            => new ConnectionFactory
            {
                UserName = configuration.Username,
                Password = configuration.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
            };       
            
       


        public async Task PublishAsync(EventTo @event)
        {
            var currentModelIndex = Interlocked.Increment(ref _currentModelIndex);

            currentModelIndex = (currentModelIndex % MaxChannelCount + MaxChannelCount) % MaxChannelCount;

            var modelLock = _modelLocks[currentModelIndex];

            await modelLock.WaitAsync(_closeToken);
            try
            {
                var channel = _models[currentModelIndex];

                var properties = _properties[currentModelIndex];

                if (channel == null)
                {
                   using var connection = await GetConnectionAsync();

                    channel = connection.CreateModel();

                    _models[currentModelIndex] = channel;

                    properties = channel.CreateBasicProperties();

                    properties.DeliveryMode = (byte)_config.DeliveryMode; // persistence

                    _properties[currentModelIndex] = properties;
                }

                var message = JsonConvert.SerializeObject(@event);

                var body = Encoding.UTF8.GetBytes(message);
                // push message to exchange
                channel.BasicPublish(_publicationAddress, properties, body);
            }
            finally
            {
                modelLock.Release();
            }
        }
        private async Task<IConnection> GetConnectionAsync()
        {
            if (_connection == null)
            {
                await _connectionLock.WaitAsync(_closeToken);
                try
                {
                    _connection ??= _config.Hostnames.Count == 0 ? _connectionFactory.CreateConnection() : _connectionFactory.CreateConnection(_config.Hostnames);
                }
                finally
                {
                    _connectionLock.Release();
                }
            }
            return _connection;
        }
        public void Dispose()
        {
            Close();

            _closeTokenSource.Dispose();

            _connectionLock.Dispose();
            foreach (var modelLock in _modelLocks)
            {
                modelLock.Dispose();
            }

            foreach (var model in _models)
            {
                model?.Dispose();
            }

            _connection?.Dispose();

            GC.SuppressFinalize(this);
        }

        private void Close()
        {
            IList<Exception> exceptions = new List<Exception>();
            try
            {
                _closeTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            for (var i = 0; i < _models.Length; i++)
            {
                try
                {
                    _modelLocks[i].Wait(10);
                    _models[i]?.Close();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                _connectionLock.Wait(10);
                _connection?.Close();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }

    }
}














//if (_config.SslOption != null)
//{
//    connectionFactory.Ssl.Version = _config.SslOption.Version;
//    connectionFactory.Ssl.CertPath = _config.SslOption.CertPath;
//    connectionFactory.Ssl.ServerName = _config.SslOption.ServerName;
//    connectionFactory.Ssl.Enabled = _config.SslOption.Enabled;
//    connectionFactory.Ssl.AcceptablePolicyErrors = _config.SslOption.AcceptablePolicyErrors;
//}
//// setup heartbeat if needed
//if (_config.Heartbeat > 0)
//    connectionFactory.RequestedHeartbeat = TimeSpan.FromMilliseconds(_config.Heartbeat);
//// only set, if has value, otherwise leave default
//if (_config.Port > 0) connectionFactory.Port = _config.Port;
//if (!string.IsNullOrEmpty(_config.VHost)) connectionFactory.VirtualHost = _config.VHost;

// return factory