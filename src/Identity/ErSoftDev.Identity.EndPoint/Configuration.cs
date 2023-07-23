using ErSoftDev.Common;
using ErSoftDev.Framework.BaseApp;
using ErSoftDev.Identity.Domain.AggregatesModel.UserAggregate;
using ErSoftDev.Identity.Infrastructure;
using ErSoftDev.Identity.Infrastructure.Repositories;

namespace ErSoftDev.Identity.EndPoint
{
    public class Configuration : BaseConfig
    {
        public Configuration(IConfiguration configuration, IHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>();
            base.ConfigureServices(services);
        }
    }
}
