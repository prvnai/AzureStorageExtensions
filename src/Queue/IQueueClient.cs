using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;

namespace Microsoft.Azure.Extensions.Storage.Queue
{
    public interface IQueueClient
    {
        Task<T> ExecuteAsync<T>(Func<CloudQueueClient, Task<T>> func);

        Task ExecuteAsync(Func<CloudQueueClient, Task> func);


    }
}
