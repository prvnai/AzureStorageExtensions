using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public class BlobClient : IBlobClient
    {
        private readonly BlobConfiguration configuration;
        private readonly StorageUri storageUri;

        public BlobClient(BlobConfiguration configuration)
        {
            this.configuration = configuration;

            this.storageUri = new StorageUri(
                  new Uri($"https://{configuration.storageAccount}.blob.core.windows.net"));

        }
        public async Task<T> ExecuteAsync<T>(Func<CloudBlobClient, Task<T>> func)
        {


            var creds = await configuration.getStorageCredsAsync();

            var operation = new BlobOperation(creds, storageUri);

            return await func(operation.cloudBlobClient);

        }

        public async Task ExecuteAsync(Func<CloudBlobClient, Task> func)
        {
            var creds = await configuration.getStorageCredsAsync();

            var operation = new BlobOperation(creds, storageUri);

            await func(operation.cloudBlobClient);
        }
    }
}
