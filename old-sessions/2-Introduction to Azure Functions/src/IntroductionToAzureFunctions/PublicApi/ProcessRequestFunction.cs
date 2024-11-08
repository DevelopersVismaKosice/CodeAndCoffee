using System.IO;
using System.Threading.Tasks;
using Common.CloudStorageProvider;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace PublicApi
{
    public class ProcessRequestFunction
    {
        private readonly ICloudStorageProvider _cloudStorageProvider;

        public ProcessRequestFunction(ICloudStorageProvider cloudStorageProvider)
        {
            _cloudStorageProvider = cloudStorageProvider;
        }

        [FunctionName("ProcessRequest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest httpRequest,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            using var reader = new StreamReader(httpRequest.Body);
            var requestBody = await reader.ReadToEndAsync();

            var request = JsonConvert.DeserializeObject<Request>(requestBody);
            if (request.A == 0 || request.B == 0)
                return new BadRequestResult();

            var queue = await _cloudStorageProvider.GetCloudQueue().ConfigureAwait(false);
            var queueMessage = new Message(request);
            await queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(queueMessage))).ConfigureAwait(false);

            return new CreatedResult($"http://localhost:7071/api/Result/{queueMessage.RequestId}", queueMessage.RequestId);
        }
    }
}
