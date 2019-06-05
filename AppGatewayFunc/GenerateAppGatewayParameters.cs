using System;
using System.IO;
using System.Threading.Tasks;
using AppGatewayFunc.Models.AppGatewayFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AppGatewayFunc
{
    public static class GenerateAppGatewayParameters
    {
        [FunctionName("GenerateAppGatewayParameters")]
        public static async Task Run(
            [QueueTrigger("applicationgateways", Connection = "AzureWebJobsStorage")]ApplicationGatewayConfigModel appGatewayConfig,
            IBinder binder,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {appGatewayConfig.description}");

            var outputBlob = await binder.BindAsync<TextWriter>(
                new BlobAttribute($"application-gateways/{appGatewayConfig.Id}.json")
                {
                    Connection = "AzureWebJobsStorage"
                });

            string json = JsonConvert.SerializeObject(appGatewayConfig);

            await outputBlob.WriteAsync(json);
        }
    }
}
