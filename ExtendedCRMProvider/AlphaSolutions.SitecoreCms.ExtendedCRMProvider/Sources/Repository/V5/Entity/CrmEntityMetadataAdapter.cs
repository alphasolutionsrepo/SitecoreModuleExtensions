using System;
using System.Linq;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata;
using CRMSecurityProvider.Sources.Attribute.Metadata;
using CRMSecurityProvider.Sources.Entity;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Entity
{
    internal class CrmEntityMetadataAdapter : CrmEntityMetadata
    {
        private readonly CrmAttributeMetadataFactory attributeMetadataFactory;
        private readonly ICrmAttributeMetadata[] attributes;
        private readonly EntityMetadata entityMetadata;

        public CrmEntityMetadataAdapter(EntityMetadata entityMetadata)
        {
            Func<AttributeMetadata, ICrmAttributeMetadata> selector = null;
            this.entityMetadata = entityMetadata;
            this.attributeMetadataFactory = new CrmAttributeMetadataFactory();
            if (entityMetadata.Attributes != null)
            {
                this.attributes =
                    entityMetadata.Attributes.Select(e => this.attributeMetadataFactory.Create(e)).ToArray();
            }
        }

        public override ICrmAttributeMetadata[] Attributes
        {
            get
            {
                return this.attributes;
            }
        }

        public override string DisplayName
        {
            get
            {
                if (((this.entityMetadata.DisplayName != null) && (this.entityMetadata.DisplayName.UserLocalizedLabel != null)) 
                    && !string.IsNullOrEmpty(this.entityMetadata.DisplayName.UserLocalizedLabel.Label))
                {
                    return this.entityMetadata.DisplayName.UserLocalizedLabel.Label;
                }
                return string.Empty;
            }
        }

        public override bool IsCustomEntity
        {
            get
            {
                if (this.entityMetadata.IsCustomEntity.HasValue)
                {
                    return this.entityMetadata.IsCustomEntity.Value;
                }
                return false;
            }
        }

        public override bool IsCustomizable
        {
            get
            {
                return this.entityMetadata.IsCustomizable.Value;
            }
        }

        public override string LogicalName
        {
            get
            {
                return this.entityMetadata.LogicalName;
            }
        }

        public override string PrimaryField
        {
            get
            {
                return this.entityMetadata.PrimaryNameAttribute;
            }
        }

        public override string PrimaryKey
        {
            get
            {
                return this.entityMetadata.PrimaryIdAttribute;
            }
        }
    }
}

