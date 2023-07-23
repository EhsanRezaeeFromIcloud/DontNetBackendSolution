using Autofac;
using Common;
using Common.Contracts;
using ErSoftDev.Common;
using ErSoftDev.Common.Contracts;
using EventBus.Base.Standard;
using EventBus.RabbitMQ.Standard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ErSoftDev.Framework.RabbitMq
{
    public static class Registration
    {
        public static IServiceCollection AddRabbitMqRegistration(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMqService>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionManager>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMqService>>();
                var appSetting = sp.GetRequiredService<IOptions<AppSetting>>();
                var lifeTimeScope = sp.GetRequiredService<ILifetimeScope>();

                var brokerName = appSetting.Value.EventBusRabbitMq.BrokerName;
                var queueName = appSetting.Value.EventBusRabbitMq.QueueName;
                var retryCount = appSetting.Value.EventBusRabbitMq.TryCount;
                var preFetchCount = appSetting.Value.EventBusRabbitMq.PreFetchCount;

                return new EventBusRabbitMqService(appSetting,
                    rabbitMqPersistentConnection,
                    eventBusSubscriptionsManager,
                    lifeTimeScope,
                    brokerName,
                    logger,
                    queueName,
                    retryCount,
                    preFetchCount
                    );
            });
            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionManager>();
            return services;
        }
    }
}
