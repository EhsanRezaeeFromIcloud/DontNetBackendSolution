using ErSoftDev.Framework.BaseApp;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ErSoftDev.ApiGateway
{
    public class Configuration : BaseConfig
    {
        public Configuration(IConfiguration configuration, IHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("ocelot.json", optional: true, reloadOnChange: true)
                .Build());
            services.AddOcelot();
            //services.AddSwaggerForOcelot(Configuration);

            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSetting appsetting)
        {
            app.UseOcelot();

            base.Configure(app, env, appsetting);
        }
    }
}
