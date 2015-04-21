using System;
using CRMSecurityProvider.Sources.Attribute;
using Sitecore.Data;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmGuidAttributeAdapter : CrmValueTypeAttributeAdapter<Guid>, ICrmGuidAttribute, ICrmAttribute<Guid>, ICrmAttribute
    {
        public CrmGuidAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, Guid internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.ToString();
        }

        protected override bool TryParseValue(string value, out Guid result)
        {
            ID id;
            if (ID.TryParse(value, out id))
            {
                result = id.Guid;
                return true;
            }
            return Guid.TryParse(value, out result);
        }
    }
}

