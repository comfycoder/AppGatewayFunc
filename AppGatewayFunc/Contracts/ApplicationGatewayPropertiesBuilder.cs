using AppGatewayFunc.Models.AppGatewayFunc.Models;
using System;
using System.Collections.Generic;

namespace GatewayConfigGenerator.Contracts
{
    public class ApplicationGatewayPropertiesBuilder
    {
        private readonly ApplicationGatewayProperties _builder;
        private readonly ApplicationGatewayConfigModel _model;

        private string _vnetId;
        private string _subnetId;
        private string _publicIPAddressId;
        private string _applicationGatewayId;

        public ApplicationGatewayPropertiesBuilder(ApplicationGatewayConfigModel model)
        {
            _builder = new ApplicationGatewayProperties();
            _model = model;
        }

        public ApplicationGatewayProperties BuildApplicationGatewayProperties()
        {
            _vnetId = $"/subscriptions/{_model.subscriptionId}/resourceGroups/{_model.publicIPAddress.vnetRGName}/providers/Microsoft.Network/virtualNetworks/{_model.publicIPAddress.vnetName}";

            _subnetId = $"{_vnetId}/subnets/{_model.publicIPAddress.subnetName}";

            _publicIPAddressId = $"/subscriptions/{_model.subscriptionId}/resourceGroups/{_model.applicationGateway.applicationGatewayRGName}/providers/Microsoft.Network/publicIPAddresses/{_model.publicIPAddress.publicIPName}";

            _applicationGatewayId = $"/subscriptions/{_model.subscriptionId}/resourceGroups/{_model.applicationGateway.applicationGatewayRGName}/providers/Microsoft.Network/applicationGateways/{_model.applicationGateway.applicationGatewayName}";

            SetGatewayIPConfiguration();

            SetSslCertificates();

            SetFrontendIPConfiguration();

            foreach (var certificate in _model.sslCertificates)
            {
                foreach (var domain in certificate.domains)
                {
                    foreach (var backendPoolModel in domain.backendPools)
                    {
                        SetBackendAddressPool(backendPoolModel);

                        SetProbe(backendPoolModel.probe);

                        SetBackendHttpsSettings(backendPoolModel.backendHttpSettings);
                    }

                    foreach (var httpListenerSet in domain.httpListeners)
                    {
                        SetHttpListener(httpListenerSet.httpListener);

                        SetHttpsListener(httpListenerSet.httpsListener);
                    }

                    foreach (var rule in domain.requestRoutingRules)
                    {
                        SetRequestRoutingRule(rule);
                    }
                }
            }

            return _builder;
        }

        private void SetSslCertificates()
        {
            foreach (var certificate in _model.sslCertificates)
            {
                var certPrefix = certificate.certificatePrefix.Replace("-", "_").ToUpper();

                var certDataName = $"__{certPrefix}_CERT_DATA__";

                var certPasswordName = $"__{certPrefix}_CERT_PASSWORD__";

                var newSslCertificate = new SslCertificate(certificate.certificateName, certDataName, certPasswordName);

                _builder.sslCertificates.Add(newSslCertificate);
            }
        }

        private void SetGatewayIPConfiguration()
        {
            var newGatewayIPConfiguration = new GatewayIPConfiguration("appGatewayIpConfig", _subnetId);

            _builder.gatewayIPConfigurations.Add(newGatewayIPConfiguration);
        }

        private void SetFrontendIPConfiguration()
        {
            var newFrontendIPConfiguration = new FrontendIPConfiguration("appGatewayFrontendIP", _publicIPAddressId);

            _builder.frontendIPConfigurations.Add(newFrontendIPConfiguration);
        }

        public void SetBackendAddressPools(List<BackendPoolModel> backendPoolModels)
        {
            foreach (var backendPoolModel in backendPoolModels)
            {
                var newBackendAddressPool = new BackendAddressPool(backendPoolModel.backendPoolName, backendPoolModel.fqdn);

                _builder.backendAddressPools.Add(newBackendAddressPool);
            }
        }

        public void SetBackendAddressPool(BackendPoolModel backendPoolModel)
        {
            var newBackendAddressPool = new BackendAddressPool(backendPoolModel.backendPoolName, backendPoolModel.fqdn);

            _builder.backendAddressPools.Add(newBackendAddressPool);
        }

        public void SetProbe(ProbeModel probeModel)
        {
            var newProbe = new Probe(probeModel.probeName, probeModel.protocol, probeModel.path,
                probeModel.interval, probeModel.timeout, probeModel.unhealthyThreshold,
                probeModel.pickHostNameFromBackendAddress, probeModel.matchStatusCodes);

            _builder.probes.Add(newProbe);
        }

        public void SetBackendHttpsSettings(BackendHttpSettingsCollectionModel httpSettings)
        {
            var probeId = $"{_applicationGatewayId}/probes/{httpSettings.probeName}";

            var newBackendHttpSettings = new BackendHttpSettings(httpSettings.httpsSettingsName,
                httpSettings.port, httpSettings.protocol, httpSettings.cookieBasedAffinity,
                httpSettings.pickHostNameFromBackendAddress, httpSettings.path, httpSettings.requestTimeout, probeId);

            _builder.backendHttpSettingsCollection.Add(newBackendHttpSettings);
        }

        public void SetHttpListener(HttpListenerModel httpListener)
        {
            var frontendIPConfigurationId = $"{_applicationGatewayId}/frontendIPConfigurations/{httpListener.frontendIPConfigurationName}";

            var frontendPortId = $"{_applicationGatewayId}/frontendPorts/{httpListener.frontendPortName}";

            var newHttpListener = new HttpListener(httpListener.httpListenerName, frontendIPConfigurationId,
                frontendPortId, httpListener.protocol, httpListener.hostName,
                httpListener.requireServerNameIndication, null);

            _builder.httpListeners.Add(newHttpListener);
        }

        public void SetHttpsListener(HttpsListenerModel httpsListener)
        {
            var frontendIPConfigurationId = $"{_applicationGatewayId}/frontendIPConfigurations/{httpsListener.frontendIPConfigurationName}";

            var frontendPortId = $"{_applicationGatewayId}/frontendPorts/{httpsListener.frontendPortName}";

            var sslCertificateId = $"{_applicationGatewayId}/sslCertificates/{httpsListener.sslCertificateName}";

            var newHttpListener = new HttpListener(httpsListener.httpsListenerName, frontendIPConfigurationId,
                frontendPortId, httpsListener.protocol, httpsListener.hostName,
                httpsListener.requireServerNameIndication, sslCertificateId);

            _builder.httpListeners.Add(newHttpListener);
        }

        private void SetRequestRoutingRule(RequestRoutingRuleModel rule)
        {
            SetHttpsRule(rule.httpsRule);

            SetHttpToHttpsRule(rule.httpToHttpsRule);
        }

        private void SetHttpsRule(HttpsRuleModel httpsRule)
        {
            var newRequestRoutingRule = new RequestRoutingRule(httpsRule.httpsRuleName, httpsRule.ruleType);

            var httpListenerId = $"{_applicationGatewayId}/httpListeners/{httpsRule.httpsListenerName}";

            newRequestRoutingRule.properties.httpListener = new HttpListenerId(httpListenerId);

            var urlPathMapId = $"{_applicationGatewayId}/urlPathMaps/{httpsRule.httpsRuleName}";

            newRequestRoutingRule.properties.urlPathMap = new UrlPathMapId(urlPathMapId);

            _builder.requestRoutingRules.Add(newRequestRoutingRule);

            SetUrlPathMaps(httpsRule);
        }

        private void SetUrlPathMaps(HttpsRuleModel httpsRule)
        {
            var defaultBackendAddressPoolId = $"{_applicationGatewayId}/backendAddressPools/{httpsRule.defaultBackendPoolName}";

            var defaultBackendHttpSettingsId = $"{_applicationGatewayId}/backendHttpSettingsCollection/{httpsRule.defaultHttpSettingsName}";

            var newUrlPathPap = new UrlPathMap(httpsRule.httpsRuleName, defaultBackendAddressPoolId, defaultBackendHttpSettingsId);

            foreach (var backendPath in httpsRule.backendPaths)
            {
                var backendAddressPoolId = $"{_applicationGatewayId}/backendAddressPools/{backendPath.backendPoolName}";

                var backendHttpSettingsId = $"{_applicationGatewayId}/backendHttpSettingsCollection/{backendPath.httpsSettingsName}";

                var pathRule = new PathRule(backendPath.pathName, backendPath.path, backendAddressPoolId, backendHttpSettingsId);

                newUrlPathPap.properties.pathRules.Add(pathRule);
            }

            _builder.urlPathMaps.Add(newUrlPathPap);
        }

        private void SetHttpToHttpsRule(HttpToHttpsRuleModel httpToHttpsRule)
        {
            var newRequestRoutingRule = new RequestRoutingRule(httpToHttpsRule.httpToHttpsRuleName, httpToHttpsRule.ruleType);

            var httpListenerId = $"{_applicationGatewayId}/httpListeners/{httpToHttpsRule.httpListenerName}";

            newRequestRoutingRule.properties.httpListener = new HttpListenerId(httpListenerId);

            var redirectConfigurationId = $"{_applicationGatewayId}/redirectConfigurations/{httpToHttpsRule.httpToHttpsRuleName}";

            newRequestRoutingRule.properties.redirectConfiguration = new RedirectConfigurationId(redirectConfigurationId);

            _builder.requestRoutingRules.Add(newRequestRoutingRule);

            SetRedirectConfiguration(httpToHttpsRule);
        }

        private void SetRedirectConfiguration(HttpToHttpsRuleModel httpToHttpsRule)
        {
            var targetListenerId = $"{_applicationGatewayId}/httpListeners/{httpToHttpsRule.targetHttpsListenerName}";

            var requestRoutingRuleId = $"{_applicationGatewayId}/requestRoutingRules/{httpToHttpsRule.httpToHttpsRuleName}";

            var redirectConfiguration = new RedirectConfiguration(httpToHttpsRule.httpToHttpsRuleName, httpToHttpsRule.redirectionType, targetListenerId, requestRoutingRuleId, httpToHttpsRule.includePath, httpToHttpsRule.includeQueryString);

            _builder.redirectConfigurations.Add(redirectConfiguration);
        }
    }
}
