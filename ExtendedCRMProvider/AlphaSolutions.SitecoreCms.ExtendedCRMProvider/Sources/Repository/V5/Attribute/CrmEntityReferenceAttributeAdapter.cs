using System;
using CRMSecurityProvider.Sources.Attribute;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmEntityReferenceAttributeAdapter : CrmAttributeAdapter<EntityReference>, ICrmReferenceAttribute, ICrmAttribute
    {
        public CrmEntityReferenceAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, EntityReference internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return base.Adaptee.Name;
        }

        public override void SetValue(string value, params string[] data)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid) && !string.IsNullOrEmpty(data[0]))
            {
                base.Adaptee.Id = guid;
                base.Adaptee.LogicalName = data[0];
            }
        }

        public Guid ReferencedEntityId
        {
            get
            {
                return base.Adaptee.Id;
            }
        }

        public string ReferencedLogicalName
        {
            get
            {
                return base.Adaptee.LogicalName;
            }
        }
    }
}

