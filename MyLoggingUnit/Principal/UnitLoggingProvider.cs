using Microsoft.AspNetCore.Builder;

namespace MyLoggingUnit.Principal
{
    public static class UnitLoggingProvider
    {
        public static MyUnitOfLog AddUnitOfLogging(this WebApplicationBuilder Builder)
        {

            if (Builder == null)
            {
                throw new ArgumentNullException(nameof(Builder));
            }
            return MyUnitOfLog.CreateUnit(Builder.Services, Builder.Configuration);
        }

    }
}
