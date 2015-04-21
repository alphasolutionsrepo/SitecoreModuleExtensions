using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmStateAttributeMetadataAdapter : CrmOptionsAttributeMetadataAdapter<StateAttributeMetadata>, ICrmStateAttributeMetadata, ICrmOptionsAttributeMetadata, ICrmAttributeMetadata
    {
        public CrmStateAttributeMetadataAdapter(StateAttributeMetadata attributeMetadata) : base(attributeMetadata)
        {
        }
    }
}

