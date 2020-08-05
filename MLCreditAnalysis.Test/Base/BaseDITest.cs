using CreditAnalysis.Service.Helpers;
using Infrastructure.Layer.Environments;
using Infrastructure.Layer.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.Helpers;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace MLCreditAnalysis.Test.Base
{
    public class BaseDITest
    {
        protected readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _projetctPath;

        public BaseDITest()
        {
            _projetctPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this._configuration = configBuilder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(this._configuration);
            services.AddOptions();
            services.AddMemoryCache();

            InfrastructureEnvironment.ContentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));

            InfrastructureDependencyInjectionHelper.Inject(services, this._configuration);
            MLServicesDependencyInjectionHelper.Inject(services, this._configuration);
            CreditAnalysisServicesDependencyInjectionHelper.Inject(services, this._configuration);

            this._serviceProvider = services.BuildServiceProvider();

            Log.Logger.Information("CreditAnalysis.Test Iniciado...");
        }

        public IServiceProvider ServiceProvider => _serviceProvider;

        public string ProjetctPath { get { return _projetctPath; } }

    }

}