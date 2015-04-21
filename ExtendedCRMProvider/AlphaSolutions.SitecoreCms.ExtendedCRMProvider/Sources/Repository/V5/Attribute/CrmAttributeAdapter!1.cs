using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal abstract class CrmAttributeAdapter<T> : CrmAttributeAdapterBase<T>
    {
        private readonly CrmAttributeCollectionAdapter crmAttributeCollection;

        protected CrmAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, T internalAttribute) : base(internalAttribute)
        {
            this.crmAttributeCollection = crmAttributeCollection;
        }

        public CrmAttributeCollectionAdapter AttributeCollection
        {
            get
            {
                return this.crmAttributeCollection;
            }
        }

        public override string Name { get; set; }
    }
}

