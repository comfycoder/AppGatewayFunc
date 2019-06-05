using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AppGatewayFunc
{
    public static class CheckHealthHttpTrigger
    {
        [FunctionName("CheckHealth")]
        public static IActionResult CheckHealth(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "checkhealth")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Check AppGatewayFunc health.");

            return (ActionResult)new OkObjectResult("SUCCESS");
        }
    }
}
