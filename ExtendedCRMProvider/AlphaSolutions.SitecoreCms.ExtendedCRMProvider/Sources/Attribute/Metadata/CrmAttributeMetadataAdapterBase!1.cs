using CRMSecurityProvider.Sources.Attribute;
using CRMSecurityProvider.Sources.Attribute.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Attribute.Metadata
{
    internal abstract class CrmAttributeMetadataAdapterBase<T> : AdapterBase<T>, ICrmAttributeMetadata
    {
        protected CrmAttributeMetadataAdapterBase(T adaptee) : base(adaptee)
        {
        }

        public abstract CrmAttributeType AttributeType { get; }

        public abstract string Description { get; }

        public abstract string DisplayName { get; }

        public abstract bool IsValidForAdvancedFind { get; }

        public abstract bool IsValidForCreate { get; }

        public abstract bool IsValidForRead { get; }

        public abstract bool IsValidForUpdate { get; }

        public abstract string LogicalName { get; }

        public abstract CrmAttributeRequiredLevel RequiredLevel { get; }

        public abstract string Title { get; }
    }
}

