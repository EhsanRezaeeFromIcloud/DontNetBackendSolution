using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Framework.Configuration;
using ErSoftDev.Framework.Middlewares;
using ErSoftDev.Framework.RabbitMq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ErSoftDev.ApiGateway
{
    public class Configuration
    {
        private readonly AppSetting _appSetting;
        private readonly string _configKey;
        public IConfiguration AppConfiguration { get; }

        public Configuration(IConfiguration appConfiguration, IHostEnvironment environment)
        {
            AppConfiguration = appConfiguration;
            _configKey = $"{nameof(AppSetting)}{environment.EnvironmentName}";
            _appSetting = appConfiguration.GetSection(_configKey).Get<AppSetting>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSetting>(AppConfiguration.GetSection(_configKey));
            services.AddSingleton(_appSetting);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddJwtAuthentication(_appSetting.Jwt);
            services.AddCustomLocalization();
            services.AddJaeger(_appSetting);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSetting appsetting)
        {
            app.UseSwagger();
            app.UseCustomRequestLocalization();
            app.UseCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwaggerForOcelotUI(options =>
            {
                options.PathToSwaggerGenerator = "/swagger/docs";

            }).UseOcelot().Wait();
            app.UseEndpoints(builder =>
            {
                builder.MapGet("/",
                    async context => { await context.Response.WriteAsync(appsetting.WelcomeNote ?? ""); });
                builder.MapControllers();
            });
        }
    }
}
