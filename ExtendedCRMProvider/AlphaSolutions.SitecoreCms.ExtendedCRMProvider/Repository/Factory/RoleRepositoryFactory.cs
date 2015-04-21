using System;
using System.Reflection;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V5;
using CRMSecurityProvider.Caching;
using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Repository;
using Microsoft.Practices.Unity;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory
{
    public class RoleRepositoryFactory : RepositoryFactory, CRMSecurityProvider.Repository.Factory.IRoleRepositoryFactory
    {
       
        public RoleRepositoryFactory()
        {
        }

        public RoleRepositoryBase GetRepository(ConfigurationSettings settings)
        {
            return base.Resolve<RoleRepositoryBase>(settings);

            Type rf = typeof (RepositoryFactory);
            FieldInfo uf = rf.GetField("unityContainer",BindingFlags.Static|BindingFlags.NonPublic);
            UnityContainer u = (UnityContainer) uf.GetValue(null);
            ResolverOverride[] r = new ResolverOverride[0];

            switch (settings.ApiVersion)
            {
                  
                case ApiVersion.V3:
                    {
                       CRMSecurityProvider.Repository.V3.CrmServiceCreatorV3 crmServiceCreatorV3 = new CRMSecurityProvider.Repository.V3.CrmServiceCreatorV3();
	
                        //return new RoleRepositoryV3(crmServiceCreatorV3.CreateService(),new MarketingListToRoleConverterV3(),new CRMSecurityProvider.Repository.V3.ContactToUserConverterV3(),
                        break;
                    }
                case ApiVersion.V4:
                    {
                        CRMSecurityProvider.Repository.V4.CrmServiceCreatorV4 crmServiceCreatorV3 = new CRMSecurityProvider.Repository.V4.CrmServiceCreatorV4();

                        //return new RoleRepositoryV3(crmServiceCreatorV4.CreateService(),new MarketingListToRoleConverterV4(),new CRMSecurityProvider.Repository.V4.ContactToUserConverterV4(),
                        break;
                    }
                case ApiVersion.V5:
                    {
                        CrmServiceCreatorV5 crmServiceCreatorV5 = new CrmServiceCreatorV5();

                        return new RoleRepositoryV5(u.Resolve<IOrganizationService>(r)
                            ,u.Resolve<CRMSecurityProvider.Repository.V5.IMarketingListToRoleConverterV5>(r)
                            ,u.Resolve<CRMSecurityProvider.Repository.V5.IContactToUserConverterV5>(r)
                            ,u.Resolve<CRMSecurityProvider.Repository.V5.UserRepositoryV5>(r)
                            ,u.Resolve<ICacheService >(r));
                    }
            }

            return null; // base.GetRepository(settings);
        }

        //protected T Resolve<T>(ConfigurationSettings settings)
        //{
        //    if (unityContainer == null)
        //    {
        //        lock (locker)
        //        {
        //            if (unityContainer == null)
        //            {
        //                UnityContainer container = new UnityContainer();
        //                switch (settings.ApiVersion)
        //                {
        //                    case ApiVersion.V3:
        //                        {
        //                            CrmServiceCreatorV3 rv = new CrmServiceCreatorV3();
        //                            container.RegisterInstance<ICrmServiceV3>(rv.CreateService(settings), new ContainerControlledLifetimeManager()).RegisterInstance<IMetadataServiceV3>(rv.CreateMetadataService(settings), new ContainerControlledLifetimeManager()).RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IContactToUserConverterV3, ContactToUserConverterV3>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IMarketingListToRoleConverterV3, MarketingListToRoleConverterV3>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<UserRepositoryBase, UserRepositoryV3>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<RoleRepositoryBase, RoleRepositoryV3>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<ProfileRepositoryBase, ProfileRepositoryV3>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<EntityRepositoryBase, EntityRepository>(new ContainerControlledLifetimeManager(), new InjectionMember[0]);
        //                            break;
        //                        }
        //                    case ApiVersion.V4:
        //                        {
        //                            CrmServiceCreatorV4 rv2 = new CrmServiceCreatorV4();
        //                            container.RegisterInstance<ICrmServiceV4>(rv2.CreateService(settings), new ContainerControlledLifetimeManager()).RegisterInstance<IMetadataServiceV4>(rv2.CreateMetadataService(settings), new ContainerControlledLifetimeManager()).RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IContactToUserConverterV4, ContactToUserConverterV4>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IMarketingListToRoleConverterV4, MarketingListToRoleConverterV4>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<UserRepositoryBase, UserRepositoryV4>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<RoleRepositoryBase, RoleRepositoryV4>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<ProfileRepositoryBase, ProfileRepositoryV4>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<EntityRepositoryBase, EntityRepository>(new ContainerControlledLifetimeManager(), new InjectionMember[0]);
        //                            break;
        //                        }
        //                    case ApiVersion.V5:
        //                        {
        //                            CrmServiceCreatorV5 rv3 = new CrmServiceCreatorV5();
        //                            container.RegisterInstance<IOrganizationService>(rv3.CreateOrganizationService(settings), new ContainerControlledLifetimeManager()).RegisterType<ICacheService, CacheService>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IContactToUserConverterV5, ContactToUserConverterV5>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<IMarketingListToRoleConverterV5, MarketingListToRoleConverterV5>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<UserRepositoryBase, UserRepositoryV5>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<RoleRepositoryBase, RoleRepositoryV5>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<ProfileRepositoryBase, ProfileRepositoryV5>(new ContainerControlledLifetimeManager(), new InjectionMember[0]).RegisterType<EntityRepositoryBase, EntityRepository>(new ContainerControlledLifetimeManager(), new InjectionMember[0]);
        //                            break;
        //                        }
        //                }
        //                unityContainer = container;
        //            }
        //        }
        //    }
        //    return unityContainer.Resolve<T>(new ResolverOverride[0]);
        //}

    }
}
