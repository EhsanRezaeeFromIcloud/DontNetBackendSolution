using Autofac;
using Autofac.Extensions.DependencyInjection;
using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Framework.Configuration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Configuration = ErSoftDev.ApiGateway.Configuration;

//var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
//(context, containerBuilder) => containerBuilder.RegisterModule(new AutofacConfigurationExtension()));

//var appSettings = builder.Configuration
//    .GetSection($"{nameof(AppSetting)}{builder.Environment.EnvironmentName}")
//    .Get<AppSetting>();

//var appConfig = new Configuration(builder.Configuration, builder.Environment);
//appConfig.ConfigureServices(builder.Services);

//var app = builder.Build();

//appConfig.Configure(app, builder.Environment, appSettings);

//await app.RunAsync();




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

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(ocelot, builder.Environment)
    .AddEnvironmentVariables();

var appSettings = builder.Configuration
    .GetSection($"{nameof(AppSetting)}{builder.Environment.EnvironmentName}")
    .Get<AppSetting>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger for ocelot
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(appSettings.Jwt);

builder.Services.AddCustomLocalization();
builder.Services.AddJaeger(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseCustomRequestLocalization();
app.UseCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";

}).UseOcelot().Wait();

app.MapControllers();

app.Run();