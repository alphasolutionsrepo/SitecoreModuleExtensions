using System.Collections.Generic;
using System.Linq;
using CRMSecurityProvider.Sources.Attribute.Metadata;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmOptionsAttributeMetadataAdapter<T> : CrmAttributeMetadataAdapter<T>, ICrmOptionsAttributeMetadata, ICrmAttributeMetadata where T: EnumAttributeMetadata
    {
        public CrmOptionsAttributeMetadataAdapter(T attributeMetadata) : base(attributeMetadata)
        {
        }

        public IEnumerable<KeyValuePair<int, string>> Options
        {
            get
            {
                return
                    base.Adaptee.OptionSet.Options.Select(
                        o =>
                            new KeyValuePair<int, string>(o.Value.HasValue ? o.Value.Value : -1,
                                o.Label.UserLocalizedLabel.Label));
            }
        }
    }
}

