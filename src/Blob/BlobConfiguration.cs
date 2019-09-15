using System;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage.Auth;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public class BlobConfiguration
    {
        internal Func<Task<StorageCredentials>> getStorageCredsAsync;
        internal string storageAccount;
        internal BlobConfiguration()
        {

        }
        

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
                var resource = $"https://{this.storageAccount}.blob.core.windows.net";

                AzureServiceTokenProvider provider = new AzureServiceTokenProvider();

                var token = await provider.GetAccessTokenAsync(resource);

                TokenCredential credential = new TokenCredential(token);
                return new StorageCredentials(credential);

            };
        }



    }
}
