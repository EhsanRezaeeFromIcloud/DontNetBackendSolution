using Autofac;
using Autofac.Extensions.DependencyInjection;
using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Framework.Configuration;
using ErSoftDev.Framework.Middlewares;
using Configuration = ErSoftDev.ApiGateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
(context, containerBuilder) => containerBuilder.RegisterModule(new AutofacConfigurationExtension()));


var appSettings = builder.Configuration
    .GetSection($"{nameof(AppSetting)}{builder.Environment.EnvironmentName}")
    .Get<AppSetting>();

var appConfig = new Configuration(builder.Configuration, builder.Environment);
appConfig.ConfigureServices(builder.Services);


var app = builder.Build();

appConfig.Configure(app, builder.Environment, appSettings);

await app.RunAsync();