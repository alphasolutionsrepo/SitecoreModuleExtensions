using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Attribute
{
    internal abstract class CrmAttributeAdapterBase<T> : AdapterBase<T>, ICrmAttribute
    {
        protected CrmAttributeAdapterBase(T internalAttribute) : base(internalAttribute)
        {
        }

        public abstract string GetStringifiedValue();
        public abstract void SetValue(string value, params string[] data);
        public override string ToString()
        {
            return this.GetStringifiedValue();
        }

        public abstract string Name { get; set; }
    }
}

