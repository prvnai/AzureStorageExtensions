using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public class BlobOperation
    {
        private StorageCredentials storageCredentials;

        public CloudBlobClient cloudBlobClient { get; private set; }

        internal BlobOperation(StorageCredentials storageCredentials, StorageUri storageUri)
        {
            this.storageCredentials = storageCredentials;


            this.cloudBlobClient = new CloudBlobClient(storageUri, storageCredentials);
        }


    }
}
