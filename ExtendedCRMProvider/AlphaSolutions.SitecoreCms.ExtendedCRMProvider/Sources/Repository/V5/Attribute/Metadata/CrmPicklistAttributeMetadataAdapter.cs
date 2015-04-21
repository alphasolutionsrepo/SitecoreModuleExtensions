using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmPicklistAttributeMetadataAdapter : CrmOptionsAttributeMetadataAdapter<PicklistAttributeMetadata>, ICrmPicklistAttributeMetadata, ICrmOptionsAttributeMetadata, ICrmAttributeMetadata
    {
        public CrmPicklistAttributeMetadataAdapter(PicklistAttributeMetadata attributeMetadata) : base(attributeMetadata)
        {
        }
    }
}

