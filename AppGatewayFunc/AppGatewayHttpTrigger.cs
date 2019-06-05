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

            PopulateApplicationGatewayConfig(input);

            var builder = new ApplicationGatewayPropertiesBuilder(input);

            var properties = builder.BuildApplicationGatewayProperties();

            return new OkObjectResult(properties);
        }

        private static void PopulateApplicationGatewayConfig(ApplicationGatewayConfigModel input)
        {
            var ag = input.applicationGateway;

            var vip = input.publicIPAddress;

            vip.publicIPName = $"{ag.applicationGatewayName}-IP";

            vip.dnsNamePrefix = ag.applicationGatewayName.ToLower();

            var certs = input.sslCertificates;

            foreach (var cert in certs)
            {
                var sslCertificateName = cert.certificateName;

                foreach (var domain in cert.domains)
                {
                    var hostName = domain.hostName;

                    domain.domainName = hostName.Replace(".", "-").ToLower();

                    domain.certificateName = cert.certificateName;

                    string defaultBackendPoolName = "";

                    string defaultHttpSettingsName = "";

                    foreach (var backendPool in domain.backendPools)
                    {
                        var appName = backendPool.appName.ToLower();

                        backendPool.appName = appName;

                        backendPool.domainName = domain.domainName;

                        backendPool.backendPoolName = $"{appName}-backend-pool";

                        if (backendPool.pathName.ToLower() == "root")
                        {
                            defaultBackendPoolName = backendPool.backendPoolName;
                        }

                        var probe = backendPool.probe;

                        probe.appName = appName;

                        probe.probeName = $"{appName}-https-probe";

                        if (backendPool.pathName.ToLower() != "root")
                        {
                            probe.path = $"/{backendPool.pathName.ToLower()}";
                        }

                        var backendHttpSettings = backendPool.backendHttpSettings;

                        backendHttpSettings.appName = backendPool.appName;

                        backendHttpSettings.httpsSettingsName = $"{backendPool.appName}-https-settings";

                        backendHttpSettings.probeName = $"{backendPool.appName}-https-probe";

                        if (backendPool.pathName.ToLower() == "root")
                        {
                            defaultHttpSettingsName = backendHttpSettings.httpsSettingsName;
                        }
                    }

                    if (domain.requestRoutingRules.Count > 0)
                    {
                        foreach (var httpListenerSet in domain.httpListeners)
                        {
                            PopulateHttpListenerSet(domain, sslCertificateName, hostName, httpListenerSet);
                        }
                    }
                    else
                    {
                        var newHttpListenerSetModel = new HttpListenerSetModel();

                        var newHttpListenerSet = PopulateHttpListenerSet(domain, sslCertificateName, hostName, newHttpListenerSetModel);

                        domain.httpListeners.Add(newHttpListenerSet);
                    }

                    if (domain.requestRoutingRules.Count > 0)
                    {
                        foreach (var routingRuleSet in domain.requestRoutingRules)
                        {
                            PopulateRoutingRuleSet(domain, defaultBackendPoolName, defaultHttpSettingsName, routingRuleSet);
                        }
                    }
                    else
                    {
                        var newRequestRoutingRuleModel = new RequestRoutingRuleModel();

                        var newRequestRoutingRules = PopulateRoutingRuleSet(domain, defaultBackendPoolName, defaultHttpSettingsName, new RequestRoutingRuleModel());

                        domain.requestRoutingRules.Add(newRequestRoutingRules);
                    }
                }
            }
        }

        private static HttpListenerSetModel PopulateHttpListenerSet(DomainModel domain, string sslCertificateName, string hostName, HttpListenerSetModel httpListenerSet)
        {
            httpListenerSet.domainName = domain.domainName;

            var httpListener = httpListenerSet.httpListener;

            httpListener.hostName = hostName;

            httpListener.httpListenerName = $"{domain.domainName}-http-listener";

            httpListenerSet.httpListener = httpListener;

            var httpsListener = httpListenerSet.httpsListener;

            httpsListener.hostName = hostName;

            httpsListener.httpsListenerName = $"{domain.domainName}-https-listener";

            httpsListener.sslCertificateName = sslCertificateName;

            httpListenerSet.httpsListener = httpsListener;

            return httpListenerSet;
        }

        private static RequestRoutingRuleModel PopulateRoutingRuleSet(DomainModel domain, string defaultBackendPoolName, string defaultHttpSettingsName, RequestRoutingRuleModel routingRuleSet)
        {
            routingRuleSet.domainName = domain.domainName;

            var httpsRule = routingRuleSet.httpsRule;

            httpsRule.httpsRuleName = $"{domain.domainName}-https-rule";

            httpsRule.httpsListenerName = $"{domain.domainName}-https-listener";

            httpsRule.defaultBackendPoolName = defaultBackendPoolName;

            httpsRule.defaultHttpSettingsName = defaultHttpSettingsName;

            foreach (var backendPool in domain.backendPools)
            {
                var newBackendPath = new BackendPathModel();

                var pathName = backendPool.pathName;

                newBackendPath.pathName = pathName;

                if (pathName.ToLower() == "root")
                {
                    newBackendPath.path = "/*";
                }
                else
                {
                    newBackendPath.path = $"/{pathName}/*";
                }

                newBackendPath.backendPoolName = backendPool.backendPoolName;

                newBackendPath.httpsSettingsName = $"{backendPool.appName}-https-settings";

                httpsRule.backendPaths.Add(newBackendPath);
            }

            routingRuleSet.httpsRule = httpsRule;

            var httpToHttpsRule = routingRuleSet.httpToHttpsRule;

            httpToHttpsRule.httpToHttpsRuleName = $"{domain.domainName}-httptohttps-rule";

            httpToHttpsRule.httpListenerName = $"{domain.domainName}-http-listener";

            httpToHttpsRule.targetHttpsListenerName = $"{domain.domainName}-https-listener";

            routingRuleSet.httpToHttpsRule = httpToHttpsRule;

            return routingRuleSet;
        }
    }
}
