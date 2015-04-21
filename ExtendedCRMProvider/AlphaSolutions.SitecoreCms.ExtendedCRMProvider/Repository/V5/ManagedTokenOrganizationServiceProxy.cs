using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Sitecore.Diagnostics;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V5
{
    public sealed class ManagedTokenOrganizationServiceProxy : OrganizationServiceProxy, IUpdateSecurityTokenRespone
    {
        private AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService> _proxyManager;

        public ManagedTokenOrganizationServiceProxy(Uri serviceUri, ClientCredentials userCredentials)
            : base(serviceUri, null, userCredentials, null)
        {
            this._proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this);
        }

        public ManagedTokenOrganizationServiceProxy(IServiceManagement<IOrganizationService> serviceManagement,
            SecurityTokenResponse securityTokenRes)
            : base(serviceManagement, securityTokenRes)
        {
            this._proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this);
        }

        public ManagedTokenOrganizationServiceProxy(IServiceManagement<IOrganizationService> serviceManagement,
            SecurityTokenResponse securityTokenRes, AuthenticationCredentials authenticationCredentials)
            : base(serviceManagement, securityTokenRes)
        {
            this._proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this, authenticationCredentials);
        }

        public ManagedTokenOrganizationServiceProxy(IServiceManagement<IOrganizationService> serviceManagement,
            ClientCredentials userCredentials)
            : base(serviceManagement, userCredentials)
        {
            this._proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this);
        }

        protected override SecurityTokenResponse AuthenticateDeviceCore()
        {
            return this._proxyManager.AuthenticateDevice();
        }

        protected override void AuthenticateCore()
        {
            this._proxyManager.PrepareCredentials();
            base.AuthenticateCore();
        }

        protected override void ValidateAuthentication()
        {
            this._proxyManager.RenewTokenIfRequired();
            base.ValidateAuthentication();
        }

        public void UpdateSecurityTokenResponse(SecurityTokenResponse securityTokenResponse)
        {
            this.SecurityTokenResponse = securityTokenResponse;
        }
    }

    public sealed class AutoRefreshSecurityToken<TProxy, TService>
        where TProxy : ServiceProxy<TService>
        where TService : class
    {
        private ClientCredentials _deviceCredentials;
        private TProxy _proxy;
        private AuthenticationCredentials _authenticationCredentials;
        private int _expirationWindow = 15;

        /// <summary>
        /// Instantiates an instance of the proxy class
        /// </summary>
        /// <param name="proxy">Proxy that will be used to authenticate the user</param>
        public AutoRefreshSecurityToken(TProxy proxy)
        {
            if (null == proxy)
            {
                throw new ArgumentNullException("proxy");
            }

            this._proxy = proxy;

            _expirationWindow = SitecoreUtility.GetSitecoreSetting<int>("AlphaSolutions.ExtendedCRMProvider.V5.TokenExpirationWindow", 15);
        }

        public AutoRefreshSecurityToken(TProxy proxy, AuthenticationCredentials authenticationCredentials) : this(proxy)
        {
            _authenticationCredentials = authenticationCredentials;
        }

        /// <summary>
        /// Prepares authentication before authen6ticated
        /// </summary>
        public void PrepareCredentials()
        {
            if (null == this._proxy.ClientCredentials)
            {
                return;
            }

            switch (this._proxy.ServiceConfiguration.AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    this._proxy.ClientCredentials.UserName.UserName = null;
                    this._proxy.ClientCredentials.UserName.Password = null;
                    break;
                case AuthenticationProviderType.Federation:
                case AuthenticationProviderType.LiveId:
                    this._proxy.ClientCredentials.Windows.ClientCredential = null;
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Authenticates the device token
        /// </summary>
        /// <returns>Generated SecurityTokenResponse for the device</returns>
        public SecurityTokenResponse AuthenticateDevice()
        {
            if (null == this._deviceCredentials)
            {
                this._deviceCredentials = DeviceIdManager.LoadOrRegisterDevice(
                    this._proxy.ServiceConfiguration.CurrentIssuer.IssuerAddress.Uri);
            }

            return this._proxy.ServiceConfiguration.AuthenticateDevice(this._deviceCredentials);
        }

        /// <summary>
        /// Renews the token (if it is near expiration or has expired)
        /// </summary>
        public void RenewTokenIfRequired()
        {
            if (null != this._proxy.SecurityTokenResponse &&
                DateTime.UtcNow.AddMinutes(15) >= this._proxy.SecurityTokenResponse.Response.Lifetime.Expires)
            {
                try
                {
                    this._proxy.Authenticate();
                }
                catch (CommunicationException communicationException)
                {
                    Log.Error("AutoRefreshSecurityToken.RenewTokeIfRequired: authentication failed.",
                        communicationException, this);
                    if (null == this._proxy.SecurityTokenResponse ||
                        DateTime.UtcNow >= this._proxy.SecurityTokenResponse.Response.Lifetime.Expires)
                    {
                        if (!RetryRenewToke())
                        {
                            throw;
                        }

                        return;
                    }

                    Log.Error(string.Concat("AutoRefreshSecurityToken.RenewTokeIfRequired: didn't retry. proxy.SecurityTokenResponse is null:", this._proxy.SecurityTokenResponse == null
                        , ", Expired:", DateTime.UtcNow >= this._proxy.SecurityTokenResponse.Response.Lifetime.Expires
                        , ", UtcNow:", DateTime.UtcNow, ", Token.Expires:", this._proxy.SecurityTokenResponse.Response.Lifetime.Expires), this);

                    // Ignore the exception 
                }
                catch (Exception exception)
                {
                    Log.Error("AutoRefreshSecurityToken.RenewTokeIfRequired: authentication failed, catch all.",
                        exception, this);

                    //TODO: if this happen figure out what to do.

                    throw;
                }

            }

            var execute = false;
            if (execute)
            {
                var result = RetryRenewToke();

                if (result)
                {
                    //do nothing
                }
            }
        }

        private bool RetryRenewToke()
        {
            try
            {
                //re authenticate
                var updateSecurityTokenResponseProxy = this._proxy as IUpdateSecurityTokenRespone;
                if (_authenticationCredentials != null && updateSecurityTokenResponseProxy != null)
                {
                    var newAuthentication = new AuthenticationCredentials();

                    newAuthentication.ClientCredentials.UserName.UserName =
                        _authenticationCredentials.ClientCredentials.UserName.UserName;
                    newAuthentication.ClientCredentials.UserName.Password =
                        _authenticationCredentials.ClientCredentials.UserName.Password;

                    var authentication =
                        this._proxy.ServiceManagement.Authenticate(newAuthentication);

                    updateSecurityTokenResponseProxy.UpdateSecurityTokenResponse(
                        authentication.SecurityTokenResponse);

                    this._proxy.Authenticate();
                    return true;
                }

                Log.Error(string.Concat("AutoRefreshSecurityToken.RenewTokeIfRequired: retry not possible. _authenticationCredentials is null:", _authenticationCredentials == null
                    , ", updateSecurityTokenResponseProxy is null:", updateSecurityTokenResponseProxy == null), this);
                return false;
            }
            catch (Exception retryException)
            {
                Log.Error("AutoRefreshSecurityToken.RenewTokeIfRequired: failed in retry authentication.", retryException, this);
                throw;
            }

            return false;
        }
    }
}

