using System;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Common;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5;
using CRMSecurityProvider.Caching;
using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Repository.V5;
using CRMSecurityProvider.Sources.Repository;
using Microsoft.Practices.Unity;
using Microsoft.Xrm.Sdk;
using CrmServiceCreatorV5 = AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V5.CrmServiceCreatorV5;
using RoleRepositoryV5 = AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V5.RoleRepositoryV5;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory
{
    public abstract class RepositoryFactory : CRMSecurityProvider.Repository.Factory.RepositoryFactory
    {
        private static readonly object Locker = new object();
        private static IUnityContainer _unityContainer;

        protected RepositoryFactory()
        {
        }

        protected T Resolve<T>(ConfigurationSettings settings)
        {
            //setting to disable customized code all together
            if (SitecoreUtility.GetSitecoreSetting<bool>("AlphaSolutions.ExtendedCRMProvider.Disable.Customizations", false))
            {
                return base.Resolve<T>(settings);
            }

            //have the original base class handle older API versions.
            if (settings.ApiVersion != ApiVersion.V5)
            {
                return base.Resolve<T>(settings);
            }

            if (_unityContainer == null)
            {
                lock (Locker)
                {
                    if (_unityContainer == null)
                    {
                        UnityContainer container = new UnityContainer();
                        switch (settings.ApiVersion)
                        {
                            case ApiVersion.V3:
                                {
                                    throw new NotImplementedException("V3 has not yet been implemented. Should be handled by base class.");
                                    break;
                                }
                            case ApiVersion.V4:
                                {
                                    throw new NotImplementedException("V3 has not yet been implemented. Should be handled by base class.");
                                    break;
                                }
                            case ApiVersion.V5:
                                {
                                    var rv3 = new CrmServiceCreatorV5();
                                    container.RegisterInstance<IOrganizationService>(
                                        rv3.CreateOrganizationService(settings),
                                        new ContainerControlledLifetimeManager())
                                        .RegisterType<ICacheService, CacheService>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType<IContactToUserConverterV5, ContactToUserConverterV5>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType<IMarketingListToRoleConverterV5, MarketingListToRoleConverterV5>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType<UserRepositoryBase, UserRepositoryV5>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType<RoleRepositoryBase, RoleRepositoryV5>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType<ProfileRepositoryBase, ProfileRepositoryV5>(
                                            new ContainerControlledLifetimeManager(), new InjectionMember[0])
                                        .RegisterType
                                        <EntityRepositoryBase,
                                            EntityRepository>(
                                                new ContainerControlledLifetimeManager(), new InjectionMember[0]);
                                    break;
                                }
                        }
                        _unityContainer = container;
                    }
                }
            }
            return _unityContainer.Resolve<T>(new ResolverOverride[0]);
        }
    }
}
