using System;
using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;
using CRMSecurityProvider.Sources.Utils.Extensions;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmDateTimeAttributeAdapter : CrmValueTypeAttributeAdapter<DateTime>, ICrmDateTimeAttribute, ICrmAttribute<DateTime>, ICrmAttribute
    {
        public CrmDateTimeAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, DateTime internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParseValue(string value, out DateTime result)
        {
            return value.TryParseDateTime(out result);
        }
    }
}

