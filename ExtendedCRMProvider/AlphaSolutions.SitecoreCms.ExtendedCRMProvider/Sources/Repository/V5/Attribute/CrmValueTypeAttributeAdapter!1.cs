using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal abstract class CrmValueTypeAttributeAdapter<T> : CrmAttributeAdapter<T>, ICrmAttribute<T>, ICrmAttribute
    {
        protected CrmValueTypeAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, T internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override void SetValue(string value, params string[] data)
        {
            T local;
            if (this.TryParseValue(value, out local))
            {
                base.AttributeCollection.SetValue(this.Name, local);
            }
        }

        protected abstract bool TryParseValue(string value, out T result);

        public T Value
        {
            get
            {
                return base.Adaptee;
            }
        }
    }
}

