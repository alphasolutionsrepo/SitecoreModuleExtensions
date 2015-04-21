
namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider
{
    public class CRMProfileProvider : CRMSecurityProvider.CRMProfileProvider
    {
        public CRMProfileProvider()
            : this(new AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory.ProfileRepositoryFactory()
            , new AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory.UserRepositoryFactory())
        {
        }

        public CRMProfileProvider(CRMSecurityProvider.Repository.Factory.IProfileRepositoryFactory profileRepositoryFactory
            , CRMSecurityProvider.Repository.Factory.IUserRepositoryFactory userRepositoryFactory)
            : base(profileRepositoryFactory, userRepositoryFactory)
        {
        }
    }
}
