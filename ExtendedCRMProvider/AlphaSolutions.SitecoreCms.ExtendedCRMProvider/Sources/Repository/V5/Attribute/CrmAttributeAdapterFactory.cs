using System;
using CRMSecurityProvider.Sources.Attribute;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmAttributeAdapterFactory : AdapterFactoryBase<object, ICrmAttribute>
    {
        private readonly CrmAttributeCollectionAdapter crmAttributeCollectionAdapter;

        public CrmAttributeAdapterFactory(CrmAttributeCollectionAdapter crmAttributeCollectionAdapter)
        {
            this.crmAttributeCollectionAdapter = crmAttributeCollectionAdapter;
        }

        public override ICrmAttribute Create(object adaptee)
        {
            if (adaptee is OptionSetValue)
            {
                return new CrmOptionSetValueAttributeAdapter(this.crmAttributeCollectionAdapter, adaptee as OptionSetValue);
            }
            if (adaptee is EntityReference)
            {
                return new CrmEntityReferenceAttributeAdapter(this.crmAttributeCollectionAdapter, adaptee as EntityReference);
            }
            if (adaptee is string)
            {
                return new CrmStringAttributeAdapter(this.crmAttributeCollectionAdapter, adaptee as string);
            }
            if (adaptee is int)
            {
                return new CrmIntegerAttributeAdapter(this.crmAttributeCollectionAdapter, (int) adaptee);
            }
            if (adaptee is long)
            {
                return new CrmLongAttributeAdapter(this.crmAttributeCollectionAdapter, (long) adaptee);
            }
            if (adaptee is bool)
            {
                return new CrmBoolAttributeAdapter(this.crmAttributeCollectionAdapter, (bool) adaptee);
            }
            if (adaptee is DateTime)
            {
                return new CrmDateTimeAttributeAdapter(this.crmAttributeCollectionAdapter, (DateTime) adaptee);
            }
            if (adaptee is decimal)
            {
                return new CrmDecimalAttributeAdapter(this.crmAttributeCollectionAdapter, (decimal) adaptee);
            }
            if (adaptee is double)
            {
                return new CrmDoubleAttributeAdapter(this.crmAttributeCollectionAdapter, (double) adaptee);
            }
            if (adaptee is Guid)
            {
                return new CrmGuidAttributeAdapter(this.crmAttributeCollectionAdapter, (Guid) adaptee);
            }
            if (adaptee is Money)
            {
                return new CrmMoneyAttributeAdapter(this.crmAttributeCollectionAdapter, adaptee as Money);
            }
            return new CrmObjectAttributeAdapter(this.crmAttributeCollectionAdapter, adaptee);
        }
    }
}

