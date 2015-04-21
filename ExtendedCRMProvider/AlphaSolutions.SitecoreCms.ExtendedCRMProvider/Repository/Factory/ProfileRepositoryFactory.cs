using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Repository.Factory;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory
{
    public class ProfileRepositoryFactory : RepositoryFactory, IProfileRepositoryFactory
    {
        public ProfileRepositoryBase GetRepository(ConfigurationSettings settings)
        {
            return base.Resolve<ProfileRepositoryBase>(settings);
        }
    }
}
