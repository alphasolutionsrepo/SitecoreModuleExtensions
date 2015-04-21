using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmStringAttributeAdapter : CrmValueTypeAttributeAdapter<string>, ICrmStringAttribute, ICrmAttribute<string>, ICrmAttribute
    {
        public CrmStringAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, string internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Value.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out string result)
        {
            result = value;
            return true;
        }
    }
}

