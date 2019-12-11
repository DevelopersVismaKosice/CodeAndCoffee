using Common.CloudStorageProvider;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Backend;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Backend
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<ICloudStorageProvider, CloudStorageProvider>();
        }
    }
}
