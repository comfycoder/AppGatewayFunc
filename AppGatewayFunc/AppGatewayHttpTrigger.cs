using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AppGatewayFunc.Models.AppGatewayFunc.Models;
using AppGatewayFunc.Models;
using GatewayConfigGenerator.Contracts;

namespace AppGatewayFunc
{
    public static class AppGatewayHttpTrigger
    {
        [FunctionName("AppGatewayHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "applicationgateways")] HttpRequest req,
            ILogger log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            log.LogInformation("Creating a new Application Gateway item.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            ApplicationGatewayConfigModel input = JsonConvert.DeserializeObject<ApplicationGatewayConfigModel>(requestBody);

            if (string.IsNullOrWhiteSpace(input.Id))
            {
                input.Id = Guid.NewGuid().ToString("n");
            }

            var agwafConfigMapper = new ApplicationGatewayConfigMapper();

            var agwafConfig = agwafConfigMapper.PopulateApplicationGatewayConfig(input);

            var builder = new ApplicationGatewayPropertiesBuilder(agwafConfig);

            var properties = builder.BuildApplicationGatewayProperties();

            return new OkObjectResult(properties);
        }
    }
}
