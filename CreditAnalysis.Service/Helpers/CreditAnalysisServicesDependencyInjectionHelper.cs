using CreditAnalysis.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditAnalysis.Service.Helpers
{
    public static class CreditAnalysisServicesDependencyInjectionHelper
    {
        public static void Inject(IServiceCollection services, IConfiguration configuration)
        {
            CreditAnalysisServicesDependencyInjectionHelper.FillInTheOptions(services, configuration);

            services.AddTransient<ICreditAnalysisService, CreditAnalysisService>();
        }

        private static void FillInTheOptions(this IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
