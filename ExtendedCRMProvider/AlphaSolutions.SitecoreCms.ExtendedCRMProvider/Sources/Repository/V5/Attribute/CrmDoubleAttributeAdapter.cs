using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmDoubleAttributeAdapter : CrmValueTypeAttributeAdapter<double>, ICrmDoubleAttribute, ICrmAttribute<double>, ICrmAttribute
    {
        public CrmDoubleAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, double internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out double result)
        {
            return double.TryParse(value, out result);
        }
    }
}

