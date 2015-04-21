using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmDecimalAttributeAdapter : CrmValueTypeAttributeAdapter<decimal>, ICrmDecimalAttribute, ICrmAttribute<decimal>, ICrmAttribute
    {
        public CrmDecimalAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, decimal internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out decimal result)
        {
            return decimal.TryParse(value, out result);
        }
    }
}

