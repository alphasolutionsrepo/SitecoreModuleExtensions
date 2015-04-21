using System.Globalization;
using System.Linq;
using CRMSecurityProvider.Sources.Attribute;
using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmOptionSetValueAttributeAdapter : CrmAttributeAdapter<OptionSetValue>, ICrmKeyValueAttribute, ICrmAttribute
    {
        public CrmOptionSetValueAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, OptionSetValue internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return this.Value;
        }

        public override void SetValue(string value, params string[] data)
        {
            int num;
            if (int.TryParse(value, out num))
            {
                base.Adaptee.Value = num;
            }
        }

        public int Key
        {
            get
            {
                return base.Adaptee.Value;
            }
        }

        public string Value
        {
            get
            {
                ICrmStateAttributeMetadata attributeMetadata =
                    base.AttributeCollection.EntityAdapter.Repository.GetAttributeMetadata(
                        base.AttributeCollection.EntityAdapter.LogicalName, this.Name) as ICrmStateAttributeMetadata;
                if (attributeMetadata == null)
                {
                    return this.Key.ToString(CultureInfo.InvariantCulture);
                }

                var attr = attributeMetadata.Options.FirstOrDefault(o => o.Key == this.Key);

                return attr.Value;
            }
        }
    }
}

