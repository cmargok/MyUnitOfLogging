using Microsoft.AspNetCore.Builder;

namespace MyLoggingUnit
{
    public static class UnitLoggingProvider 
    {

       /* public static MyUnitOfLog AddUnitOfLogging(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<JsonConfiguration>();

            return MyUnitOfLog.CreateUnit(services);
        }
        */

        public static MyUnitOfLog AddUnitOfLoggingV2(this WebApplicationBuilder Builder) {

            if (Builder == null)
            {
                throw new ArgumentNullException(nameof(Builder));
            }
            return MyUnitOfLog.CreateUnit(Builder.Services, Builder.Configuration);
        }




    }
}
