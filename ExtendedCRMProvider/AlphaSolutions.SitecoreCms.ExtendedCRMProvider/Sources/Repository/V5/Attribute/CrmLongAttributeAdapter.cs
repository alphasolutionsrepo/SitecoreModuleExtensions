using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmLongAttributeAdapter : CrmValueTypeAttributeAdapter<long>, ICrmLongAttribute, ICrmAttribute<long>, ICrmAttribute
    {
        public CrmLongAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, long internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out long result)
        {
            return long.TryParse(value, out result);
        }
    }
}

