using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common.CloudStorageProvider
{
    public interface ICloudStorageProvider
    {
        Task<CloudTable> GetCloudTable();
        Task<CloudQueue> GetCloudQueue();
    }
}
