using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GatewayConfigGenerator.Contracts
{
    // Sku

    public class Sku
    {
        public string name { get; set; }

        public string tier { get; set; }

        public int capacity { get; set; }

        public Sku()
        {
            this.name = "Waf_v2";
            this.tier = "Waf_v2";
            this.capacity = 2;
        }

        public Sku(string name, string tier, int capacity)
        {
            this.name = name;
            this.tier = tier;
            this.capacity = capacity;
        }
    }

    // GatewayIPConfiguration

    public class Subnet
    {
        public string id { get; set; }

        public Subnet(string id)
        {
            this.id = id;
        }
    }

    public class GatewayIPConfigurationProperties
    {
        public Subnet subnet { get; set; }

        public GatewayIPConfigurationProperties(string id)
        {
            this.subnet = new Subnet(id);
        }
    }

    public class GatewayIPConfiguration
    {
        public string name { get; set; }

        public GatewayIPConfigurationProperties properties { get; set; }

        public GatewayIPConfiguration(string name, string subnetId)
        {
            this.name = name;
            this.properties = new GatewayIPConfigurationProperties(subnetId);
        }
    }

    // SslCertificate

    public class SslCertificateProperties
    {
        public string data { get; set; }

        public string password { get; set; }

        public SslCertificateProperties(string data, string password)
        {
            this.data = data;
            this.password = password;
        }
    }

    public class SslCertificate
    {
        public string name { get; set; }

        public SslCertificateProperties properties { get; set; }

        public SslCertificate(string name, string data, string password)
        {
            this.name = name;
            this.properties = new SslCertificateProperties(data, password);
        }
    }

    // FrontendIPConfiguration

    public class PublicIPAddress
    {
        public string id { get; set; }

        public PublicIPAddress(string id)
        {
            this.id = id;
        }
    }

    public class FrontendIPConfigurationProperties
    {
        public PublicIPAddress PublicIPAddress { get; set; }

        public FrontendIPConfigurationProperties(string id)
        {
            this.PublicIPAddress = new PublicIPAddress(id);
        }
    }

    public class FrontendIPConfiguration
    {
        public string name { get; set; }

        public FrontendIPConfigurationProperties properties { get; set; }

        public FrontendIPConfiguration(string name, string id)
        {
            this.name = name;
            this.properties = new FrontendIPConfigurationProperties(id);
        }
    }

    // FrontendPort

    public class FrontendPortProperties
    {
        public int Port { get; set; }

        public FrontendPortProperties()
        {
            this.Port = 80;
        }

        public FrontendPortProperties(int port)
        {
            this.Port = port;
        }
    }

    public class FrontendPort
    {
        public string name { get; set; }

        public FrontendPortProperties properties { get; set; }

        public FrontendPort(string name, int port)
        {
            this.name = name;

            this.properties = new FrontendPortProperties(port);
        }
    }

    // BackendAddressPool

    public class BackendAddress
    {
        public string fqdn { get; set; }

        public BackendAddress(string fqdn)
        {
            this.fqdn = fqdn;
        }
    }

    public class BackendAddressPoolProperties
    {
        public List<BackendAddress> BackendAddresses { get; set; }

        public BackendAddressPoolProperties(string fqdn)
        {
            this.BackendAddresses = new List<BackendAddress>();

            this.BackendAddresses.Add(new BackendAddress(fqdn));
        }

        public BackendAddressPoolProperties(ICollection<string> fqdns)
        {
            this.BackendAddresses = new List<BackendAddress>();

            foreach (var fqdn in fqdns)
            {
                this.BackendAddresses.Add(new BackendAddress(fqdn));
            }
        }
    }

    public class BackendAddressPool
    {
        public string name { get; set; }

        public BackendAddressPoolProperties properties { get; set; }

        public BackendAddressPool(string name, string fqdn)
        {
            this.name = name;

            this.properties = new BackendAddressPoolProperties(fqdn);
        }

        public BackendAddressPool(string name, ICollection<string> fqdns)
        {
            this.name = name;

            this.properties = new BackendAddressPoolProperties(fqdns);
        }
    }

    // BackendHttpSettings

    public class ProbeId
    {
        public string id { get; set; }

        public ProbeId(string id)
        {
            this.id = id;
        }
    }

    public class BackendHttpSettingsProperties
    {
        public int Port { get; set; }

        public string Protocol { get; set; }

        public string CookieBasedAffinity { get; set; }

        public bool pickHostNameFromBackendAddress { get; set; }

        public int requestTimeout { get; set; }

        public ProbeId probe { get; set; }

        public BackendHttpSettingsProperties(int port, string protocol, string cookieBasedAffinity,
            bool pickHostNameFromBackendAddress, int requestTimeout, string probeId)
        {
            this.Port = port;
            this.Protocol = protocol;
            this.CookieBasedAffinity = cookieBasedAffinity;
            this.pickHostNameFromBackendAddress = pickHostNameFromBackendAddress;
            this.requestTimeout = requestTimeout;
            this.probe = new ProbeId(probeId);
        }
    }

    public class BackendHttpSettings
    {
        public string name { get; set; }

        public BackendHttpSettingsProperties properties { get; set; }

        public BackendHttpSettings(string name, int port, string protocol, string cookieBasedAffinity,
            bool pickHostNameFromBackendAddress, int requestTimeout, string probeId)
        {
            this.name = name;
            this.properties = new BackendHttpSettingsProperties(port, protocol, 
                cookieBasedAffinity, pickHostNameFromBackendAddress, requestTimeout, probeId);
        }
    }

    // HttpListener

    public class FrontendIPConfigurationId
    {
        public string id { get; set; }

        public FrontendIPConfigurationId(string id)
        {
            this.id = id;
        }
    }

    public class FrontendPortId
    {
        public string id { get; set; }

        public FrontendPortId(string id)
        {
            this.id = id;
        }
    }

    public class SslCertificateId
    {
        public string id { get; set; }

        public SslCertificateId(string id)
        {
            this.id = id;
        }
    }

    public class HttpListenerProperties
    {
        public FrontendIPConfigurationId FrontendIPConfiguration { get; set; }

        public FrontendPortId FrontendPort { get; set; }

        public string Protocol { get; set; }

        public string hostName { get; set; }

        public bool requireServerNameIndication { get; set; }

        public SslCertificateId sslCertificateId { get; set; }

        public HttpListenerProperties(string frontendIPConfigurationId, string frontendPortId,
            string protocol, string hostName, bool requireServerNameIndication, string sslCertificateId = null)
        {
            this.FrontendIPConfiguration = new FrontendIPConfigurationId(frontendIPConfigurationId);
            this.FrontendPort = new FrontendPortId(frontendPortId);
            this.Protocol = protocol;
            this.hostName = hostName;
            this.requireServerNameIndication = requireServerNameIndication;
            if (!string.IsNullOrWhiteSpace(sslCertificateId))
            {
                this.sslCertificateId = new SslCertificateId(sslCertificateId);
            }
        }
    }

    public class HttpListener
    {
        public string name { get; set; }

        public HttpListenerProperties properties { get; set; }

        public HttpListener(string name, string frontendIPConfigurationId, string frontendPortId,
            string protocol, string hostName, bool requireServerNameIndication, string sslCertificateId = null)
        {
            this.name = name;

            this.properties = new HttpListenerProperties(frontendIPConfigurationId, frontendPortId,
                protocol, hostName, requireServerNameIndication, sslCertificateId);
        }
    }

    // UrlPathMap

    public class DefaultBackendAddressPoolId
    {
        public string id { get; set; }

        public DefaultBackendAddressPoolId(string id)
        {
            this.id = id;
        }
    }

    public class DefaultBackendHttpSettingsId
    {
        public string id { get; set; }

        public DefaultBackendHttpSettingsId(string id)
        {
            this.id = id;
        }
    }

    public class BackendHttpSettingsId
    {
        public string id { get; set; }

        public BackendHttpSettingsId(string id)
        {
            this.id = id;
        }
    }

    public class BackendAddressPoolId
    {
        public string id { get; set; }

        public BackendAddressPoolId(string id)
        {
            this.id = id;
        }
    }

    public class PathRuleProperties
    {
        public List<string> paths { get; set; }

        public BackendAddressPoolId backendAddressPool { get; set; }

        public BackendHttpSettingsId backendHttpSettings { get; set; }

        public PathRuleProperties(string paths, string backendAddressPoolId, string backendHttpSettingsId)
        {
            char[] delimiterChars = { ' ', ',', '\t' };

            string[] arrPaths = paths.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

            this.paths = new List<string>();

            foreach (string item in arrPaths)
            {
                this.paths.Add(item);
            }

            this.backendAddressPool = new BackendAddressPoolId(backendAddressPoolId);

            this.backendHttpSettings = new BackendHttpSettingsId(backendHttpSettingsId);
        }
    }

    public class PathRule
    {
        public string name { get; set; }

        public PathRuleProperties properties { get; set; }

        public PathRule(string name, string paths, string backendAddressPoolId, 
            string backendHttpSettingsId)
        {
            this.name = name;

            this.properties = new PathRuleProperties(paths, backendAddressPoolId, backendHttpSettingsId);
        }
    }

    public class UrlPathMapProperties
    {
        public DefaultBackendAddressPoolId defaultBackendAddressPool { get; set; }

        public DefaultBackendHttpSettingsId defaultBackendHttpSettings { get; set; }

        public List<PathRule> pathRules { get; set; }

        public UrlPathMapProperties(string defaultBackendAddressPoolId, string defaultBackendHttpSettingsId)
        {
            this.defaultBackendAddressPool = new DefaultBackendAddressPoolId(defaultBackendAddressPoolId);

            this.defaultBackendHttpSettings = new DefaultBackendHttpSettingsId(defaultBackendHttpSettingsId);

            this.pathRules = new List<PathRule>();
        }
    }

    public class UrlPathMap
    {
        public string name { get; set; }

        public UrlPathMapProperties properties { get; set; }

        public UrlPathMap(string name, string defaultBackendAddressPoolId, string defaultBackendHttpSettingsId)
        {
            this.name = name;

            this.properties = new UrlPathMapProperties(defaultBackendAddressPoolId, defaultBackendHttpSettingsId);
        }
    }

    // RequestRoutingRule

    public class HttpListenerId
    {
        public string id { get; set; }

        public HttpListenerId(string id)
        {
            this.id = id;
        }
    }

    public class UrlPathMapId
    {
        public string id { get; set; }

        public UrlPathMapId(string id)
        {
            this.id = id;
        }
    }

    public class RedirectConfigurationId
    {
        public string id { get; set; }

        public RedirectConfigurationId(string id)
        {
            this.id = id;
        }
    }
    
    public class RequestRoutingRuleProperties
    {
        public string RuleType { get; set; }

        public HttpListenerId httpListener { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public UrlPathMapId urlPathMap { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RedirectConfigurationId redirectConfiguration { get; set; }

        public RequestRoutingRuleProperties(string ruleType)
        {
            this.RuleType = ruleType;
        }
    }

    public class RequestRoutingRule
    {
        public string name { get; set; }

        public RequestRoutingRuleProperties properties { get; set; }

        public RequestRoutingRule(string name, string ruleType)
        {
            this.name = name;

            this.properties = new RequestRoutingRuleProperties(ruleType);
        }
    }

    // Probe

    public class Match
    {
        public List<string> statusCodes { get; set; }

        public Match(string match)
        {
            this.statusCodes = new List<string>();

            char[] delimiterChars = { ' ', ',', '\t' };

            string[] arrStatusCodes = match.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in arrStatusCodes)
            {
                this.statusCodes.Add(item);
            }            
        }
    }

    public class ProbeProperties
    {
        public string protocol { get; set; }

        public string path { get; set; }

        public int interval { get; set; }

        public int timeout { get; set; }

        public int unhealthyThreshold { get; set; }

        public bool pickHostNameFromBackendHttpSettings { get; set; }

        public int minServers { get; set; }

        public Match match { get; set; }

        public ProbeProperties(string protocol, string path, int interval, int timeout, int unhealthyThreshold,
            bool pickHostNameFromBackendHttpSettings, string statusCodes)
        {
            this.protocol = protocol;
            this.path = path;
            this.interval = interval;
            this.timeout = timeout;
            this.unhealthyThreshold = unhealthyThreshold;
            this.pickHostNameFromBackendHttpSettings = pickHostNameFromBackendHttpSettings;
            this.match = new Match(statusCodes);
        }
    }

    public class Probe
    {
        public string name { get; set; }

        public ProbeProperties properties { get; set; }

        public Probe(string name, string protocol, string path, int interval, int timeout, int unhealthyThreshold,
            bool pickHostNameFromBackendHttpSettings, string statusCodes)
        {
            this.name = name;

            this.properties = new ProbeProperties(protocol, path, interval, timeout, unhealthyThreshold,
                pickHostNameFromBackendHttpSettings, statusCodes);
        }
    }

    // RedirectConfiguration

    public class TargetListenerId
    {
        public string id { get; set; }

        public TargetListenerId(string id)
        {
            this.id = id;
        }
    }

    public class RequestRoutingRuleId
    {
        public string id { get; set; }

        public RequestRoutingRuleId(string id)
        {
            this.id = id;
        }
    }

    public class RedirectConfigurationProperties
    {
        public string redirectType { get; set; }

        public TargetListenerId targetListener { get; set; }

        public bool includePath { get; set; }

        public bool includeQueryString { get; set; }

        public List<RequestRoutingRuleId> requestRoutingRules { get; set; }

        public RedirectConfigurationProperties(string redirectType, string targetListenerId, 
            string requestRoutingRuleId, bool includePath, bool includeQueryString)
        {
            this.redirectType = redirectType;
            this.targetListener = new TargetListenerId(targetListenerId);
            this.requestRoutingRules = new List<RequestRoutingRuleId>();
            this.requestRoutingRules.Add(new RequestRoutingRuleId(requestRoutingRuleId));
            this.includePath = includePath;
            this.includeQueryString = includeQueryString;
        }
    }

    public class RedirectConfiguration
    {
        public string name { get; set; }

        public RedirectConfigurationProperties properties { get; set; }

        public RedirectConfiguration(string name, string redirectType, string targetListenerId,
            string requestRoutingRuleId, bool includePath, bool includeQueryString)
        {
            this.name = name;

            this.properties = new RedirectConfigurationProperties(redirectType, targetListenerId, 
                requestRoutingRuleId, includePath, includeQueryString);
        }
    }

    // SslPolicy

    public class SslPolicy
    {
        public string policyType { get; set; }

        public string policyName { get; set; }

        public SslPolicy()
        {
            this.policyType = "Predefined";
            this.policyName = "AppGwSslPolicy20170401";
        }
    }

    //WebApplicationFirewallConfiguration

    public class WebApplicationFirewallConfiguration
    {
        public bool enabled { get; set; }

        public string firewallMode { get; set; }

        public string ruleSetType { get; set; }

        public string ruleSetVersion { get; set; }

        public bool requestBodyCheck { get; set; }

        public int maxRequestBodySizeInKb { get; set; }

        public int fileUploadLimitInMb { get; set; }

        public WebApplicationFirewallConfiguration()
        {
            this.enabled = true;
            this.firewallMode = "Prevention";
            this.ruleSetType = "OWASP";
            this.ruleSetVersion = "3.0";
            this.requestBodyCheck = false;
            this.maxRequestBodySizeInKb = 128;
            this.fileUploadLimitInMb = 100;
        }
    }

    // AutoscaleConfiguration

    public class AutoscaleConfiguration
    {
        public int minCapacity { get; set; }

        public int maxCapacity { get; set; }

        public AutoscaleConfiguration()
        {
            this.minCapacity = 2;
            this.maxCapacity = 10;
        }
    }


    // Root Object (Application Gateway Properties)

    public class ApplicationGatewayProperties
    {
        public Sku sku { get; set; }

        public List<GatewayIPConfiguration> gatewayIPConfigurations { get; set; }

        public List<SslCertificate> sslCertificates { get; set; }

        public List<FrontendIPConfiguration> frontendIPConfigurations { get; set; }

        public List<FrontendPort> frontendPorts { get; set; }

        public List<BackendAddressPool> backendAddressPools { get; set; }

        public List<Probe> probes { get; set; }

        public List<HttpListener> httpListeners { get; set; }

        public List<BackendHttpSettings> backendHttpSettingsCollection { get; set; }

        public List<RequestRoutingRule> requestRoutingRules { get; set; }

        public List<UrlPathMap> urlPathMaps { get; set; }

        public List<RedirectConfiguration> redirectConfigurations { get; set; }

        public SslPolicy sslPolicy { get; set; }

        public bool enableHttp2 { get; set; }

        public WebApplicationFirewallConfiguration webApplicationFirewallConfiguration { get; set; }

        public AutoscaleConfiguration autoscaleConfiguration { get; set; }

        public ApplicationGatewayProperties()
        {
            this.sku = new Sku();

            this.gatewayIPConfigurations = new List<GatewayIPConfiguration>();

            this.sslCertificates = new List<SslCertificate>();

            this.frontendIPConfigurations = new List<FrontendIPConfiguration>();

            this.frontendPorts = new List<FrontendPort>();
            this.frontendPorts.Add(new FrontendPort("appGatewayFrontendPort80", 80));
            this.frontendPorts.Add(new FrontendPort("appGatewayFrontendPort443", 443));

            this.backendAddressPools = new List<BackendAddressPool>();

            this.probes = new List<Probe>();

            this.httpListeners = new List<HttpListener>();

            this.backendHttpSettingsCollection = new List<BackendHttpSettings>();

            this.requestRoutingRules = new List<RequestRoutingRule>();

            this.urlPathMaps = new List<UrlPathMap>();

            this.redirectConfigurations = new List<RedirectConfiguration>();

            this.sslPolicy = new SslPolicy();

            this.enableHttp2 = true;

            this.webApplicationFirewallConfiguration = new WebApplicationFirewallConfiguration();

            this.autoscaleConfiguration = new AutoscaleConfiguration();
        }
    }

    public class ApplicationGatewayRootObject
    {
        public ApplicationGatewayProperties properties { get; set; }

        public ApplicationGatewayRootObject()
        {
            this.properties = new ApplicationGatewayProperties();
        }
    }

}
