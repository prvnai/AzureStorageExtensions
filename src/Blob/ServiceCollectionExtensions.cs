using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBlob(this IServiceCollection services, Action<BlobConfiguration> options)
        {
            var configuration = new BlobConfiguration();

            options(configuration);

            services.AddTransient<IBlobClient, BlobClient>((sp) => new BlobClient(configuration));

        }

    }
}
