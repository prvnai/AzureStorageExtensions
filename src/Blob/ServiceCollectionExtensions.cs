using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureQueue(this IServiceCollection services, Action<BlobConfiguration> queueoptions)
        {
            var configuration = new BlobConfiguration();

            queueoptions(configuration);

            services.AddTransient<IBlobClient, BlobClient>((sp) => new BlobClient(configuration));

        }

    }
}
