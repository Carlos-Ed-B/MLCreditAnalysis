using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Layer.Helpers
{
    public static class InfrastructureDependencyInjectionHelper
    {
        public static void Inject(IServiceCollection services, IConfiguration configuration)
        {
        }

        public static void FillInTheOptions(this IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
