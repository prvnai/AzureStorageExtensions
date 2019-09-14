using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Queue;

namespace Microsoft.Azure.Extensions.Storage.Queue
{
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
}
