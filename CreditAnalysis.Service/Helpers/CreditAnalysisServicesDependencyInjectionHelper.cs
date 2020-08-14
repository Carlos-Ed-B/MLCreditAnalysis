using CreditAnalysis.Model;
using CreditAnalysis.Model.Interfaces;
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
            var environmentsOptionBase = new EnvironmentsOptionBase()
            {
                BaseUri = configuration.GetSection(nameof(EnvironmentsOptionBase)).GetValue<string>("BaseUri"),
            };

            services.AddTransient<IEnvironmentsOptionBase, EnvironmentsOptionBase>(config =>
            {
                return environmentsOptionBase;
            });
        }
    }
}
