using System;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Queue;
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

    public interface IQueueClient
    {
        Task<T> ExecuteAsync<T>(Func<CloudQueueClient, Task<T>> func);

        Task ExecuteAsync(Func<CloudQueueClient, Task> func);


    }

    public class QueueClient : IQueueClient
    {
        private readonly QueueConfiguration configuration;
        private readonly StorageUri storageUri;

        public QueueClient(QueueConfiguration configuration)
        {
            this.configuration = configuration;

            this.storageUri = new StorageUri(
                  new Uri($"https://{configuration.storageAccount}.queue.core.windows.net"));

        }
        public async Task<T> ExecuteAsync<T>(Func<CloudQueueClient, Task<T>> func)
        {


            var creds = await configuration.getStorageCredsAsync();

            var operation = new QueueOperation(creds, storageUri);

            return await func(operation.cloudQueueClient);

        }

        public async Task ExecuteAsync(Func<CloudQueueClient, Task> func)
        {
            var creds = await configuration.getStorageCredsAsync();

            var operation = new QueueOperation(creds, storageUri);

            await func(operation.cloudQueueClient);
        }
    }

    public class QueueOperation
    {
        private StorageCredentials storageCredentials;

        public CloudQueueClient cloudQueueClient { get; private set; }

        internal QueueOperation(StorageCredentials storageCredentials, StorageUri storageUri)
        {
            this.storageCredentials = storageCredentials;


            this.cloudQueueClient = new Azure.Storage.Queue.CloudQueueClient(storageUri, storageCredentials);
        }


    }

    public class QueueConfiguration
    {
        internal Func<Task<StorageCredentials>> getStorageCredsAsync;
        internal string storageAccount;
        internal QueueConfiguration()
        {

        }
        public string QueueName { get; set; }

        public void SetAuthenticationWithAccessKey(string storageAccount, string accessKey)
        {
            this.storageAccount = storageAccount;
            getStorageCredsAsync = () => Task.FromResult(new StorageCredentials(storageAccount, accessKey));
        }

        public void SetAuthenticationWithSas(string storageAccount, string sasToken)
        {
            this.storageAccount = storageAccount;
            getStorageCredsAsync = () => Task.FromResult(new StorageCredentials(sasToken));
        }

        public void SetAuthenticationViaManagedServiceIdentity(string storageAccount)
        {
            this.storageAccount = storageAccount;
            getStorageCredsAsync = async () =>
            {
                var resource = $"https://{this.storageAccount}.queue.core.windows.net";

                AzureServiceTokenProvider provider = new AzureServiceTokenProvider();

                var token = await provider.GetAccessTokenAsync(resource);

                TokenCredential credential = new TokenCredential(token);
                return new StorageCredentials(credential);

            };
        }



    }
}
