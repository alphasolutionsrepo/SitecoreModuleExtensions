using System;
using System.Collections.Generic;
using System.Linq;

using CRMSecurityProvider.Caching;
using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Repository.Factory;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Sources.Repository;
using Microsoft.Practices.Unity;
using System.Reflection;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.Sitecore.ExtendedCRMProvider.RoleRepository
{
    public class RoleRepositoryFactory : CRMSecurityProvider.Repository.Factory.IRoleRepositoryFactory
    {
       
        public RoleRepositoryFactory()
        {
        }

        public RoleRepositoryBase GetRepository(ConfigurationSettings settings)
        {
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
                        CRMSecurityProvider.Repository.V5.CrmServiceCreatorV5 crmServiceCreatorV5 = new CRMSecurityProvider.Repository.V5.CrmServiceCreatorV5();

                        return new RoleRepositoryV5(u.Resolve<IOrganizationService>(r),u.Resolve<CRMSecurityProvider.Repository.V5.IMarketingListToRoleConverterV5>(r),u.Resolve<CRMSecurityProvider.Repository.V5.IContactToUserConverterV5>(r),u.Resolve<CRMSecurityProvider.Repository.V5.UserRepositoryV5>(r),u.Resolve<ICacheService >(r));
                    }
            }

            return null; // base.GetRepository(settings);
        }
    }
}
