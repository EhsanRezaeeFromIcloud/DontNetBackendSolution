using Autofac;
using Autofac.Extensions.DependencyInjection;
using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Framework.Configuration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Configuration = ErSoftDev.ApiGateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
(context, containerBuilder) => containerBuilder.RegisterModule(new AutofacConfigurationExtension()));

var appConfig = new Configuration(builder.Configuration, builder.Environment);
appConfig.ConfigureServices(builder.Services);

var ocelot = "Ocelot";

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = ocelot;
});
builder.Services.AddOcelot(builder.Configuration).AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var appSettings = builder.Configuration
    .GetSection($"{nameof(AppSetting)}{builder.Environment.EnvironmentName}")
    .Get<AppSetting>();

var app = builder.Build();
appConfig.Configure(app, builder.Environment, appSettings);
app.Run();