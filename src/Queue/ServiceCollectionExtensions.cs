using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Extensions.Storage.Queue
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureQueue(this IServiceCollection services, Action<QueueConfiguration> queueoptions)
        {
            var configuration = new QueueConfiguration();

            queueoptions(configuration);

            services.AddTransient<IQueueClient, QueueClient>((sp) => new QueueClient(configuration));

        }

    }
}
