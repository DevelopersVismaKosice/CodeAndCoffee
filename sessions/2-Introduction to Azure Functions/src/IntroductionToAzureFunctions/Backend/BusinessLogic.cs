using System.Threading.Tasks;
using Common.CloudStorageProvider;
using Common.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Backend
{
    public class BusinessLogic
    {
        private readonly ICloudStorageProvider _cloudStorageProvider;

        public BusinessLogic(ICloudStorageProvider cloudStorageProvider)
        {
            _cloudStorageProvider = cloudStorageProvider;
        }

        [FunctionName("BusinessLogic")]
        public async Task Run([QueueTrigger("queue-name", Connection = "AzureWebJobsStorage")]Message message, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message}");

            var table = await _cloudStorageProvider.GetCloudTable().ConfigureAwait(false);
            await table.ExecuteAsync(TableOperation.InsertOrMerge(
                new Result(message.RequestId, message.Request.A + message.Request.B)));
        }
    }
}
