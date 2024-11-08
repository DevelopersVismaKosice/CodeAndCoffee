using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common.CloudStorageProvider
{
    public class CloudStorageProvider : ICloudStorageProvider
    {
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private const string TableName = "tablename";
        private const string QueueName = "queue-name";

        public async Task<CloudTable> GetCloudTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference(TableName);

            await cloudTable.CreateIfNotExistsAsync();

            return cloudTable;
        }

        public async Task<CloudQueue> GetCloudQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference(QueueName);

            await queue.CreateIfNotExistsAsync();

            return queue;
        }
    }
}
