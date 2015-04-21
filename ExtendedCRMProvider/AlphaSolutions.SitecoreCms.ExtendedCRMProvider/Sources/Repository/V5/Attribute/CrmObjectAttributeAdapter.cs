namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmObjectAttributeAdapter : CrmAttributeAdapter<object>
    {
        public CrmObjectAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, object internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString();
        }

        public override void SetValue(string value, params string[] data)
        {
            base.AttributeCollection.SetValue(this.Name, value);
        }
    }
}

