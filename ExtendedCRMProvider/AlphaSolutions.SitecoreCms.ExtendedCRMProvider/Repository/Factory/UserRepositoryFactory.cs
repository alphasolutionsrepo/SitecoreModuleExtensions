using CRMSecurityProvider.Configuration;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Repository.Factory;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory
{
    public class UserRepositoryFactory : RepositoryFactory, IUserRepositoryFactory
    {
        public UserRepositoryBase GetRepository(ConfigurationSettings settings)
        {
            return base.Resolve<UserRepositoryBase>(settings);
        }
    }
}
