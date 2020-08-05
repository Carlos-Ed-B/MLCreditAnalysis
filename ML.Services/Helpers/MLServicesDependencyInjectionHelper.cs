using IBM.Cloud.SDK.Core.Authentication.Iam;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.Azure;
using ML.Services.Azure.Environments;
using ML.Services.Azure.Environments.Interfaces;
using ML.Services.Azure.Interfaces;
using ML.Services.IBM;
using ML.Services.IBM.Environments;
using ML.Services.IBM.Environments.Interfaces;

namespace ML.Services.Helpers
{
    public static class MLServicesDependencyInjectionHelper
    {
        public static void Inject(IServiceCollection services, IConfiguration configuration)
        {
            MLServicesDependencyInjectionHelper.FillInTheOptions(services, configuration);

            services.AddTransient<IIBMVisualRecognitionService, IBMVisualRecognitionService>();
            services.AddTransient<IAzureVisualRecognitionService, AzureVisualRecognitionService>();
        }

        private static void FillInTheOptions(this IServiceCollection services, IConfiguration configuration)
        {

            var azureEnvironment = new AzureEnvironment()
            {
                VisualRecognitionApiKey = configuration.GetSection(nameof(AzureEnvironment)).GetValue<string>("VisualRecognitionApiKey"),
                VisualRecognitionApiUrl = configuration.GetSection(nameof(AzureEnvironment)).GetValue<string>("VisualRecognitionApiUrl"),
            };

            services.AddTransient<IAzureEnvironment, AzureEnvironment>(config =>
            {
                return azureEnvironment;
            });

            var ibmEnvironment = new IBMEnvironment()
            {
               VisualRecognitionApiKey  = configuration.GetSection(nameof(IBMEnvironment)).GetValue<string>("VisualRecognitionApiKey"),
                VisualRecognitionVersionDate = configuration.GetSection(nameof(IBMEnvironment)).GetValue<string>("VisualRecognitionVersionDate"),
            };

            services.AddTransient<IIBMEnvironment, IBMEnvironment>(config =>
            {
                return ibmEnvironment;
            });

            var ibmIamAuthenticator = new IBMIamAuthenticator()
            {
                Authenticator = new IamAuthenticator(apikey: ibmEnvironment.VisualRecognitionApiKey)              
            };

            services.AddTransient<IIBMIamAuthenticator, IBMIamAuthenticator>(config =>
            {
                return ibmIamAuthenticator;
            });
        }
    }
}


