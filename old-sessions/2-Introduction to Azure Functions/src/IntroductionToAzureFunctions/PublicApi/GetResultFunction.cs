using System.Threading.Tasks;
using Common.CloudStorageProvider;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace PublicApi
{
    public class GetResultFunction
    {
        private readonly ICloudStorageProvider _cloudStorageProvider;

        public GetResultFunction(ICloudStorageProvider cloudStorageProvider)
        {
            _cloudStorageProvider = cloudStorageProvider;
        }

        [FunctionName("GetResult")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "result/{requestId}")] HttpRequest httpRequest,
            string requestId,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var table = await _cloudStorageProvider.GetCloudTable().ConfigureAwait(false);

            var getOperation = TableOperation.Retrieve<Result>("default", requestId);
            var tableResult = await table.ExecuteAsync(getOperation);
            var result = (Result)tableResult.Result;
            if (result == null)
                return new NotFoundResult();
            return new OkObjectResult(new {Sum = result.Sum});
        }
    }
}