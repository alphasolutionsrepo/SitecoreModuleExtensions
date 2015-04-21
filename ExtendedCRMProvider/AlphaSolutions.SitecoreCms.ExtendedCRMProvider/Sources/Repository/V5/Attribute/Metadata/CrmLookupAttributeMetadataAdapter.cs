using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmLookupAttributeMetadataAdapter : CrmAttributeMetadataAdapter<LookupAttributeMetadata>, ICrmLookupAttributeMetadata, ICrmAttributeMetadata
    {
        public CrmLookupAttributeMetadataAdapter(LookupAttributeMetadata lookupAttributeMetadata) : base(lookupAttributeMetadata)
        {
        }

        public string[] Targets
        {
            get
            {
                return base.Adaptee.Targets;
            }
        }
    }
}

