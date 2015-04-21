using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    public class CrmAttributeMetadataFactory
    {
        public ICrmAttributeMetadata Create(AttributeMetadata attributeMetadata)
        {
            if (attributeMetadata is LookupAttributeMetadata)
            {
                return new CrmLookupAttributeMetadataAdapter(attributeMetadata as LookupAttributeMetadata);
            }
            if (attributeMetadata is StateAttributeMetadata)
            {
                return new CrmStateAttributeMetadataAdapter(attributeMetadata as StateAttributeMetadata);
            }
            if (attributeMetadata is StatusAttributeMetadata)
            {
                return new CrmStatusAttributeMetadataAdapter(attributeMetadata as StatusAttributeMetadata);
            }
            if (attributeMetadata is PicklistAttributeMetadata)
            {
                return new CrmPicklistAttributeMetadataAdapter(attributeMetadata as PicklistAttributeMetadata);
            }
            return new CrmAttributeMetadataAdapter<AttributeMetadata>(attributeMetadata);
        }
    }
}
