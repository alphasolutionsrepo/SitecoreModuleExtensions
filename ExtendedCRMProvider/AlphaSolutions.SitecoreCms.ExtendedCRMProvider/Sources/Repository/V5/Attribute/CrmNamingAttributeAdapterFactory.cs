using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmNamingAttributeAdapterFactory : CrmAttributeAdapterFactory
    {
        public CrmNamingAttributeAdapterFactory(CrmAttributeCollectionAdapter crmAttributeCollectionAdapter) : base(crmAttributeCollectionAdapter)
        {
        }

        public ICrmAttribute Create(string attributeName, object adaptee)
        {
            ICrmAttribute attribute = base.Create(adaptee);
            attribute.Name = attributeName;
            return attribute;
        }
    }
}

