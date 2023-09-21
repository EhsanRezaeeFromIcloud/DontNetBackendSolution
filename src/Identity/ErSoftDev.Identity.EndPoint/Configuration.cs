﻿using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Identity.Infrastructure;

namespace ErSoftDev.Identity.EndPoint
{
    public class Configuration : BaseConfig
    {
        public Configuration(IConfiguration configuration, IHostEnvironment environment) : base(configuration, environment)
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>();
            base.ConfigureServices(services);
        }
    }
}
