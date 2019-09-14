using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace Microsoft.Azure.Extensions.Storage.Queue
{
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
}
