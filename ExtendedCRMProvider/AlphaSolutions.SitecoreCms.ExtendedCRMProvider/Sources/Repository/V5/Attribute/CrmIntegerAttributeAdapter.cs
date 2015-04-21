using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmIntegerAttributeAdapter : CrmValueTypeAttributeAdapter<int>, ICrmIntegerAttribute, ICrmAttribute<int>, ICrmAttribute
    {
        public CrmIntegerAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, int internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out int result)
        {
            return int.TryParse(value, out result);
        }
    }
}

