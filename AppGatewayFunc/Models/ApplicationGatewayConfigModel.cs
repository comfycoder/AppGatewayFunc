using System.Collections.Generic;

namespace AppGatewayFunc.Models
{
    using System;
    using System.Collections.Generic;

    namespace AppGatewayFunc.Models
    {
        public class ApplicationGatewayModel
        {
            public string applicationGatewayName { get; set; }
            public string applicationGatewayRGName { get; set; }
            public string location { get; set; }
            public string tier { get; set; }
            public int capacity { get; set; }
            public int autoscaleMaxCapacity { get; set; }

            public ApplicationGatewayModel()
            {
                this.applicationGatewayName = "";
                this.applicationGatewayName = "";
                this.location = "eastus2";
                this.tier = "WAF_v2";
                this.capacity = 2;
                this.autoscaleMaxCapacity = 10;
            }
        }

        public class PublicIPAddressModel
        {
            public string publicIPName { get; set; }
            public string vnetName { get; set; }
            public string vnetRGName { get; set; }
            public string subnetName { get; set; }
            public int idleTimeout { get; set; }
            public string dnsNamePrefix { get; set; }

            public PublicIPAddressModel()
            {
                this.publicIPName = "";
                this.vnetName = "";
                this.vnetRGName = "";
                this.subnetName = "";
                this.idleTimeout = 4;
                this.dnsNamePrefix = "";
            }
        }

        public class SslCertificateModel
        {
            public string certificateName { get; set; }
            public string certificatePrefix { get; set; }
            public List<DomainModel> domains { get; set; }

            public SslCertificateModel()
            {
                this.certificateName = "";
                this.certificatePrefix = "";
                this.domains = new List<DomainModel>();
            }
        }

        public class DomainModel
        {
            public string domainName { get; set; }
            public string hostName { get; set; }
            public string certificateName { get; set; }

            public List<BackendPoolModel> backendPools { get; set; }
            public List<HttpListenerSetModel> httpListeners { get; set; }
            public List<RequestRoutingRuleModel> requestRoutingRules { get; set; }

            public DomainModel()
            {
                this.domainName = "";
                this.hostName = "";
                this.certificateName = "";

                this.backendPools = new List<BackendPoolModel>();
                this.httpListeners = new List<HttpListenerSetModel>();
                this.requestRoutingRules = new List<RequestRoutingRuleModel>();
            }
        }

        public class BackendPoolModel
        {
            public string appName { get; set; }
            public string backendPoolName { get; set; }
            public string fqdn { get; set; }
            public string pathName { get; set; }
            public string domainName { get; set; }

            public ProbeModel probe { get; set; }
            public BackendHttpSettingsCollectionModel backendHttpSettings { get; set; }

            public BackendPoolModel()
            {
                this.appName = "";
                this.backendPoolName = "";
                this.fqdn = "";
                this.pathName = "root";
                this.domainName = "";

                this.probe = new ProbeModel();
                this.backendHttpSettings = new BackendHttpSettingsCollectionModel();
            }
        }

        public class ProbeModel
        {
            public string appName { get; set; }
            public string probeName { get; set; }
            public string protocol { get; set; }
            public string path { get; set; }
            public int interval { get; set; }
            public int timeout { get; set; }
            public int unhealthyThreshold { get; set; }
            public bool pickHostNameFromBackendAddress { get; set; }
            public int minServers { get; set; }
            public string matchStatusCodes { get; set; }

            public ProbeModel()
            {
                this.appName = "";
                this.probeName = "";
                this.protocol = "Https";
                this.path = "/";
                this.interval = 120;
                this.timeout = 30;
                this.unhealthyThreshold = 3;
                this.pickHostNameFromBackendAddress = true;
                this.minServers = 0;
                this.matchStatusCodes = "200-399";
            }
        }

        public class HttpListenerModel
        {
            public string httpListenerName { get; set; }
            public string frontendIPConfigurationName { get; set; }
            public string frontendPortName { get; set; }
            public string hostName { get; set; }
            public string protocol { get; set; }
            public bool requireServerNameIndication { get; set; }

            public HttpListenerModel()
            {
                this.httpListenerName = "";
                this.frontendIPConfigurationName = "appGatewayFrontendIP";
                this.frontendPortName = "appGatewayFrontendPort80";
                this.hostName = "";
                this.protocol = "Http";
                this.requireServerNameIndication = true;
            }
        }

        public class HttpsListenerModel
        {
            public string httpsListenerName { get; set; }
            public string frontendIPConfigurationName { get; set; }
            public string frontendPortName { get; set; }
            public string hostName { get; set; }
            public string protocol { get; set; }
            public bool requireServerNameIndication { get; set; }
            public string sslCertificateName { get; set; }

            public HttpsListenerModel()
            {
                this.httpsListenerName = "";
                this.frontendIPConfigurationName = "appGatewayFrontendIP";
                this.frontendPortName = "appGatewayFrontendPort443";
                this.hostName = "";
                this.protocol = "Https";
                this.requireServerNameIndication = true;
                this.sslCertificateName = "";
            }
        }

        public class HttpListenerSetModel
        {
            public string domainName { get; set; }
            public HttpListenerModel httpListener { get; set; }
            public HttpsListenerModel httpsListener { get; set; }

            public HttpListenerSetModel()
            {
                this.domainName = "";
                this.httpListener = new HttpListenerModel();
                this.httpsListener = new HttpsListenerModel();
            }
        }

        public class BackendHttpSettingsCollectionModel
        {
            public string appName { get; set; }
            public string httpsSettingsName { get; set; }
            public int port { get; set; }
            public string protocol { get; set; }
            public string cookieBasedAffinity { get; set; }
            public bool pickHostNameFromBackendAddress { get; set; }
            public string path { get; set; }
            public int requestTimeout { get; set; }
            public string probeName { get; set; }

            public BackendHttpSettingsCollectionModel()
            {
                this.appName = "";
                this.httpsSettingsName = "";
                this.port = 443;
                this.protocol = "Https";
                this.cookieBasedAffinity = "Disabled";
                this.pickHostNameFromBackendAddress = true;
                this.path = "/";
                this.requestTimeout = 20;
                this.probeName = "";
            }
        }

        public class BackendPathModel
        {
            public string pathName { get; set; }
            public string path { get; set; }
            public string backendPoolName { get; set; }
            public string httpsSettingsName { get; set; }

            public BackendPathModel()
            {
                this.pathName = "";
                this.path = "/";
                this.backendPoolName = "";
                this.httpsSettingsName = "";
            }
        }

        public class HttpsRuleModel
        {
            public string httpsRuleName { get; set; }
            public string ruleType { get; set; }
            public string httpsListenerName { get; set; }
            public string defaultBackendPoolName { get; set; }
            public string defaultHttpSettingsName { get; set; }
            public List<BackendPathModel> backendPaths { get; set; }

            public HttpsRuleModel()
            {
                this.httpsRuleName = "";
                this.ruleType = "PathBasedRouting";
                this.httpsListenerName = "";
                this.defaultBackendPoolName = "";
                this.defaultHttpSettingsName = "";
                this.backendPaths = new List<BackendPathModel>();
            }
        }

        public class HttpToHttpsRuleModel
        {
            public string httpToHttpsRuleName { get; set; }
            public string ruleType { get; set; }
            public string httpListenerName { get; set; }
            public string redirectionType { get; set; }
            public string targetHttpsListenerName { get; set; }
            public bool includePath { get; set; }
            public bool includeQueryString { get; set; }

            public HttpToHttpsRuleModel()
            {
                this.httpToHttpsRuleName = "";
                this.ruleType = "Basic";
                this.httpListenerName = "";
                this.redirectionType = "Temporary";
                this.targetHttpsListenerName = "";
                this.includePath = true;
                this.includeQueryString = true;
            }
        }

        public class RequestRoutingRuleModel
        {
            public string domainName { get; set; }
            public HttpsRuleModel httpsRule { get; set; }
            public HttpToHttpsRuleModel httpToHttpsRule { get; set; }

            public RequestRoutingRuleModel()
            {
                this.domainName = "";
                this.httpsRule = new HttpsRuleModel();
                this.httpToHttpsRule = new HttpToHttpsRuleModel();
            }
        }

        public class ApplicationGatewayConfigModel
        {
            public string Id { get; set; }
            public string description { get; set; }
            public string subscriptionId { get; set; }
            public ApplicationGatewayModel applicationGateway { get; set; }
            public PublicIPAddressModel publicIPAddress { get; set; }
            public List<SslCertificateModel> sslCertificates { get; set; }

                                  

            public ApplicationGatewayConfigModel()
            {
                this.Id = Guid.NewGuid().ToString("n");
                this.description = "";
                this.subscriptionId = "";
                this.applicationGateway = new ApplicationGatewayModel();
                this.publicIPAddress = new PublicIPAddressModel();
                this.sslCertificates = new List<SslCertificateModel>();

            }
        }

        public class RootObject
        {
            public ApplicationGatewayConfigModel applicationGatewayConfig { get; set; }
        }
    }

}
