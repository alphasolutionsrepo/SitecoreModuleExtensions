using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Attribute.Metadata;
using CRMSecurityProvider.Sources.Attribute;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata
{
    internal class CrmAttributeMetadataAdapter<T> : CrmAttributeMetadataAdapterBase<T> where T: AttributeMetadata
    {
        public CrmAttributeMetadataAdapter(T attributeMetadata) : base(attributeMetadata)
        {
        }

        public override CrmAttributeType AttributeType
        {
            get
            {
                if (!base.Adaptee.AttributeType.HasValue)
                {
                    return CrmAttributeType.Empty;
                }
                AttributeTypeCode valueOrDefault = base.Adaptee.AttributeType.GetValueOrDefault();
                if (base.Adaptee.AttributeType.HasValue)
                {
                    switch (valueOrDefault)
                    {
                        case AttributeTypeCode.Boolean:
                            return CrmAttributeType.Boolean;

                        case AttributeTypeCode.Customer:
                            return CrmAttributeType.Customer;

                        case AttributeTypeCode.DateTime:
                            return CrmAttributeType.DateTime;

                        case AttributeTypeCode.Decimal:
                            return CrmAttributeType.Decimal;

                        case AttributeTypeCode.Double:
                            return CrmAttributeType.Double;

                        case AttributeTypeCode.Integer:
                            return CrmAttributeType.Integer;

                        case AttributeTypeCode.Lookup:
                            return CrmAttributeType.Lookup;

                        case AttributeTypeCode.Memo:
                            return CrmAttributeType.Memo;

                        case AttributeTypeCode.Money:
                            return CrmAttributeType.Money;

                        case AttributeTypeCode.Owner:
                            return CrmAttributeType.Owner;

                        case AttributeTypeCode.PartyList:
                            return CrmAttributeType.PartyList;

                        case AttributeTypeCode.Picklist:
                            return CrmAttributeType.Picklist;

                        case AttributeTypeCode.State:
                            return CrmAttributeType.State;

                        case AttributeTypeCode.Status:
                            return CrmAttributeType.Status;

                        case AttributeTypeCode.String:
                            return CrmAttributeType.String;

                        case AttributeTypeCode.Uniqueidentifier:
                            return CrmAttributeType.UniqueIdentifier;

                        case AttributeTypeCode.CalendarRules:
                            return CrmAttributeType.CalendarRules;

                        case AttributeTypeCode.Virtual:
                            return CrmAttributeType.Virtual;

                        case AttributeTypeCode.BigInt:
                            return CrmAttributeType.BigInt;

                        case AttributeTypeCode.ManagedProperty:
                            return CrmAttributeType.ManagedProperty;

                        case AttributeTypeCode.EntityName:
                            return CrmAttributeType.EntityName;
                    }
                }
                return CrmAttributeType.Unknown;
            }
        }

        public override string Description
        {
            get
            {
                if ((base.Adaptee.Description != null) && (base.Adaptee.Description.UserLocalizedLabel != null))
                {
                    return base.Adaptee.Description.UserLocalizedLabel.Label;
                }
                return string.Empty;
            }
        }

        public override string DisplayName
        {
            get
            {
                if (((base.Adaptee.DisplayName != null) && (base.Adaptee.DisplayName.UserLocalizedLabel != null)) && !string.IsNullOrEmpty(base.Adaptee.DisplayName.UserLocalizedLabel.Label))
                {
                    return base.Adaptee.DisplayName.UserLocalizedLabel.Label;
                }
                return string.Empty;
            }
        }

        public override bool IsValidForAdvancedFind
        {
            get
            {
                return base.Adaptee.IsValidForAdvancedFind.Value;
            }
        }

        public override bool IsValidForCreate
        {
            get
            {
                if (base.Adaptee.IsValidForCreate.HasValue)
                {
                    return base.Adaptee.IsValidForCreate.Value;
                }
                return false;
            }
        }

        public override bool IsValidForRead
        {
            get
            {
                if (base.Adaptee.IsValidForRead.HasValue)
                {
                    return base.Adaptee.IsValidForRead.Value;
                }
                return false;
            }
        }

        public override bool IsValidForUpdate
        {
            get
            {
                if (base.Adaptee.IsValidForUpdate.HasValue)
                {
                    return base.Adaptee.IsValidForUpdate.Value;
                }
                return false;
            }
        }

        public override string LogicalName
        {
            get
            {
                return base.Adaptee.LogicalName;
            }
        }

        public override CrmAttributeRequiredLevel RequiredLevel
        {
            get
            {
                if (base.Adaptee.RequiredLevel == null)
                {
                    return CrmAttributeRequiredLevel.Empty;
                }
                switch (base.Adaptee.RequiredLevel.Value)
                {
                    case AttributeRequiredLevel.None:
                        return CrmAttributeRequiredLevel.None;

                    case AttributeRequiredLevel.SystemRequired:
                        return CrmAttributeRequiredLevel.SystemRequired;

                    case AttributeRequiredLevel.ApplicationRequired:
                        return CrmAttributeRequiredLevel.ApplicationRequired;

                    case AttributeRequiredLevel.Recommended:
                        return CrmAttributeRequiredLevel.Recommended;
                }
                return CrmAttributeRequiredLevel.Unknown;
            }
        }

        public override string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DisplayName))
                {
                    return this.DisplayName;
                }
                return this.LogicalName;
            }
        }
    }
}

