using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace Microsoft.Azure.Extensions.Storage.Blob
{
    public interface IBlobClient
    {
        Task<T> ExecuteAsync<T>(Func<CloudBlobClient, Task<T>> func);

        Task ExecuteAsync(Func<CloudBlobClient, Task> func);


    }
}
