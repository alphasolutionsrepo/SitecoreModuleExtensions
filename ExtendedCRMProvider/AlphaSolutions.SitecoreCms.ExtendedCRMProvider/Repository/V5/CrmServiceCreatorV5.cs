using System;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Common;
using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Utils;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Sitecore.Diagnostics;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V5
{
    public class CrmServiceCreatorV5
    {
        public IOrganizationService CreateOrganizationService(ConfigurationSettings settings)
        {
            Assert.ArgumentNotNull(settings, "settings");
            ConditionalLog.Info("CreateOrganizationService(settings). Started.", this, TimerAction.Start, "createOrganizationService");
            IOrganizationService service = null;
            try
            {
                IServiceManagement<IOrganizationService> serviceManagement = ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(settings.Url));
                AuthenticationCredentials authenticationCredentials = new AuthenticationCredentials();
                switch (serviceManagement.AuthenticationType)
                {
                    case AuthenticationProviderType.ActiveDirectory:
                        authenticationCredentials.ClientCredentials.Windows.ClientCredential = CrmHelper.CreateNetworkCredential(settings.User, settings.Password);
                        service = new OrganizationServiceProxy(serviceManagement, authenticationCredentials.ClientCredentials);
                        break;

                    case AuthenticationProviderType.Federation:
                    case AuthenticationProviderType.LiveId:
                    case AuthenticationProviderType.OnlineFederation:
                    {
                        authenticationCredentials.ClientCredentials.UserName.UserName = settings.User;
                        authenticationCredentials.ClientCredentials.UserName.Password = settings.Password;
                        
                        AuthenticationCredentials credentials2 = serviceManagement.Authenticate(authenticationCredentials);
                        
                        //setting to be able to configure using the custom token service proxy
                        if (
                            SitecoreUtility.GetSitecoreSetting<bool>(
                                "AlphaSolutions.ExtendedCRMProvider.V5.Disable.AuthenticationCustomization", false))
                        {
                            service = new CRMSecurityProvider.Repository.V5.ManagedTokenOrganizationServiceProxy(serviceManagement, credentials2.SecurityTokenResponse);
                        }
                        else
                        {
                            service = new ManagedTokenOrganizationServiceProxy(serviceManagement,
                                credentials2.SecurityTokenResponse, authenticationCredentials);
                        }
                        break;
                    }
                }

                if (service == null)
                {
                    ConditionalLog.Error("CreateOrganizationService(settings). service could not be initialized.", this);
                    return null;
                }

                service.Execute(new WhoAmIRequest());
                ConditionalLog.Info("CreateOrganizationService(settings). CRM organization service has been created.", this, TimerAction.Tick, "createOrganizationService");
            }
            catch (Exception exception)
            {
                ConditionalLog.Error("Couldn't create CRM organization service.", exception, this);
                return null;
            }
            finally
            {
                ConditionalLog.Info("CreateOrganizationService(settings). Finished.", this, TimerAction.Stop, "createOrganizationService");
            }
            return service;
        }
    }
}

