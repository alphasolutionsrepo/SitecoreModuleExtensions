using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Repository.Factory;
using CRMSecurityProvider.Utils;
using Sitecore.Diagnostics;
namespace AlphaSolutions.Sitecore.ExtendedCRMProvider
{
    public class CRMRoleProvider : CRMSecurityProvider.CRMRoleProvider
    {
        public CRMRoleProvider()
            : this(new RoleRepository.RoleRepositoryFactory())
        {
        }

        public CRMRoleProvider(IRoleRepositoryFactory roleRepositoryFactory) : base(roleRepositoryFactory)
        {
        }


    }
}
