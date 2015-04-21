using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;
using Sitecore;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmBoolAttributeAdapter : CrmValueTypeAttributeAdapter<bool>, ICrmBoolAttribute, ICrmAttribute<bool>, ICrmAttribute
    {
        public CrmBoolAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, bool internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out bool result)
        {
            result = MainUtil.GetBool(value, false);
            return true;
        }
    }
}

