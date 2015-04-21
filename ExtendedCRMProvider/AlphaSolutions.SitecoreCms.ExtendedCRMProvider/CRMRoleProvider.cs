
namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider
{
    public class CRMRoleProvider : CRMSecurityProvider.CRMRoleProvider
    {
        public CRMRoleProvider()
            : this(new AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.Factory.RoleRepositoryFactory())
        {
        }

        public CRMRoleProvider(CRMSecurityProvider.Repository.Factory.IRoleRepositoryFactory roleRepositoryFactory)
            : base(roleRepositoryFactory)
        {
        }


    }
}
