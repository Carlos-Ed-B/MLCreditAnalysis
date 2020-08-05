using CreditAnalysis.Service.Helpers;
using Infrastructure.Layer.Environments;
using Infrastructure.Layer.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ML.Services.Helpers;
using Serilog;
using System.IO;
using System.Reflection;

namespace MLCreditAnalysis.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddOptions();
            services.AddMemoryCache();

            try
            {
                InfrastructureEnvironment.ContentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));
            }
            catch (System.Exception)
            {
                InfrastructureEnvironment.ContentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }


            InfrastructureDependencyInjectionHelper.Inject(services, this.Configuration);
            MLServicesDependencyInjectionHelper.Inject(services, this.Configuration);
            CreditAnalysisServicesDependencyInjectionHelper.Inject(services, this.Configuration);

            Log.Logger.Information("CreditAnalysis.Test Iniciado...");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
