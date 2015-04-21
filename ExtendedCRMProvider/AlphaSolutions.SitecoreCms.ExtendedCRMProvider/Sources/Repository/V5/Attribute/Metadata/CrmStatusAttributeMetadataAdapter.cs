using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmStatusAttributeMetadataAdapter : CrmOptionsAttributeMetadataAdapter<StatusAttributeMetadata>, ICrmStatusAttributeMetadata, ICrmOptionsAttributeMetadata, ICrmAttributeMetadata
    {
        public CrmStatusAttributeMetadataAdapter(StatusAttributeMetadata attributeMetadata) : base(attributeMetadata)
        {
        }
    }
}

