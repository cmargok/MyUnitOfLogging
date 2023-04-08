using Microsoft.Extensions.DependencyInjection;
using UnitOfLogging.Core;
namespace UnitOfLogging
{
    public static class LoggingUnitOfMeasurement
    {
        public static UnitOfLog UseUnitOfLogging(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return new UnitOfLog(services);
        }
    }

}
