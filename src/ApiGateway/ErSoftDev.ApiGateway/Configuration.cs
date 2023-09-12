using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Framework.Configuration;
using ErSoftDev.Framework.Middlewares;
using ErSoftDev.Framework.RabbitMq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ErSoftDev.ApiGateway
{
    public class Configuration //: BaseConfig
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

        public /*override*/ void ConfigureServices(IServiceCollection services)
        {

            //services.AddCustomMediatr();
            //services.AddGrpc();
            //services.AddHttpClient();
            services.Configure<AppSetting>(AppConfiguration.GetSection(_configKey));
            services.AddSingleton(_appSetting);
            //services.AddRabbitMqConnection(_appSetting);
            //services.AddRabbitMqRegistration(_appSetting);
            //services.AddCustomHangfire(_appSetting);
            //services.AddCustomLocalization();
            //services.AddApplicationDbContext(_appSetting);
            //services.AddMinimalMvc();
            //services.ApiGatewayAddJwtAuthentication(_appSetting.Jwt);
            //services.AddCustomApiVersioning();
            //services.AddCustomSwaggerGen(_appSetting.Swagger);
            //services.AddControllers();
            //services.AddJaeger(_appSetting);


            //services.AddSingleton<IConfiguration>(provider => new ConfigurationBuilder()
            //    .AddEnvironmentVariables()
            //    .AddJsonFile("ocelot.json", optional: true, reloadOnChange: true)
            //    .Build());

            //services.AddOcelot();
            //services.AddSwaggerForOcelot(AppConfiguration);


            //base.ConfigureServices(services);

        }

        public /*override*/ void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSetting appsetting)
        {
            //app.UseCustomRequestLocalization();
            //app.UseCustomExceptionMiddleware();
            ////app.UseRateLimitationMiddleware();
            //app.UseHstsNotInDevelopment(env);
            //app.UseHttpsRedirection();
            //app.UseCustomSwaggerUi(_appSetting.Swagger);
            //app.UseAuthentication();
            //app.UseMvc();
            //app.UseCustomStaticFile();
            //app.UseRouting();
            //app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
            ////app.UseHangfireDashboard();
            //app.UseEndpoints(builder =>
            //{
            //    builder.MapGet("/",
            //        async context => { await context.Response.WriteAsync(appsetting./*Value.*/WelcomeNote ?? ""); });
            //    builder.MapControllers();
            //    builder.UseGrpcEndPoint();
            //    //builder.MapHangfireDashboard();
            //});
            //app.UseOcelot();
            //app.UseSwaggerForOcelotUI();

            //base.Configure(app, env, appsetting);
        }
    }
}
