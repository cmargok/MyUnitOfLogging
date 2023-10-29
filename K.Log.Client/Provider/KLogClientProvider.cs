using K.Logger.Client.Client;
using Microsoft.Extensions.DependencyInjection;
namespace K.Logger.Client.Provider
{
    public static class KLogClientProvider
    {
        public static KLog AddKLogClient(this IServiceCollection services)
        {

            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return KLog.CreateClient(services);
        }
    }
}
