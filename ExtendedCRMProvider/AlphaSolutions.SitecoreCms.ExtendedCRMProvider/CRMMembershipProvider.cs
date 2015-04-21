
namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider
{
    public class CRMMembershipProvider : CRMSecurityProvider.CRMMembershipProvider
    {
        public CRMMembershipProvider()
            : this(new AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory.UserRepositoryFactory()
            , new AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory.ProfileRepositoryFactory())
        {
        }

        public CRMMembershipProvider(CRMSecurityProvider.Repository.Factory.IUserRepositoryFactory userRepositoryFactory
            , CRMSecurityProvider.Repository.Factory.IProfileRepositoryFactory profileRepositoryFactory) 
            : base(userRepositoryFactory, profileRepositoryFactory)
        {
        }

 

    }
}
