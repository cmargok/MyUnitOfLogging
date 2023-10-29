using Serilog;


namespace K.Logger.Client
{

    public class KlogClient : IKlog
    {
        private readonly ILogger _logger;
        public KlogClient(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(string message)
        {
            _logger.Information(message);
        }
    }

    public interface IKlog
    { 
        
        public void Log(string message);
        
    }
}
