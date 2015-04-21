using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Entity;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Entity
{
    internal class CrmEntityAdapter : CrmEntityAdapterBase<Microsoft.Xrm.Sdk.Entity>
    {
        private readonly CrmAttributeCollectionAdapter _crmAttributeCollectionAdapter;

        public CrmEntityAdapter(EntityRepository repository, Microsoft.Xrm.Sdk.Entity entity) : base(repository, entity)
        {
            this._crmAttributeCollectionAdapter = new CrmAttributeCollectionAdapter(this, base.Adaptee.Attributes);
            base.AttributeCollectionAdapterInitialized();
            base.Id = entity.Id;
        }

        internal CrmAttributeCollectionAdapter AttributeCollectionAdapter
        {
            get
            {
                return this._crmAttributeCollectionAdapter;
            }
        }

        public override ICrmAttributeCollection Attributes
        {
            get
            {
                return this._crmAttributeCollectionAdapter;
            }
        }

        public override string LogicalName
        {
            get
            {
                return base.Adaptee.LogicalName;
            }
            set
            {
                base.Adaptee.LogicalName = value;
            }
        }
    }
}

