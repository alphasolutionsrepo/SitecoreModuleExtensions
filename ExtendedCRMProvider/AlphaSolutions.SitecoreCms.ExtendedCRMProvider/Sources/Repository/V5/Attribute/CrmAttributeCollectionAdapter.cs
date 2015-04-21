using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Entity;
using CRMSecurityProvider.Sources.Attribute;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmAttributeCollectionAdapter : AdapterBase<AttributeCollection>, ICrmAttributeCollection, IEnumerable<KeyValuePair<string, ICrmAttribute>>, IEnumerable
    {
        private readonly CrmNamingAttributeAdapterFactory crmAttributeAdapterFactory;
        private readonly CrmEntityAdapter entityAdapter;
        private readonly string[] systemNames;

        public CrmAttributeCollectionAdapter(CrmEntityAdapter entityAdapter, AttributeCollection attributeCollection) : base(attributeCollection)
        {
            this.systemNames = new string[] { "statecode", "statuscode" };
            this.entityAdapter = entityAdapter;
            this.crmAttributeAdapterFactory = new CrmNamingAttributeAdapterFactory(this);
        }

        public void Add(KeyValuePair<string, object> attribute)
        {
            if (base.Adaptee.ContainsKey(attribute.Key))
            {
                base.Adaptee[attribute.Key] = attribute.Value;
            }
            else
            {
                base.Adaptee.Add(attribute.Key, attribute.Value);
            }
        }

        public void AddRange(KeyValuePair<string, object>[] attributes)
        {
            foreach (KeyValuePair<string, object> pair in attributes)
            {
                this.Add(pair);
            }
        }

        public ICrmAttribute Create(string name, CrmAttributeType type, string value, params string[] data)
        {
            object minValue;
            switch (type)
            {
                case CrmAttributeType.Boolean:
                    minValue = false;
                    break;

                case CrmAttributeType.Customer:
                case CrmAttributeType.Lookup:
                case CrmAttributeType.Owner:
                {
                    EntityReference reference = new EntityReference {
                        Name = name
                    };
                    minValue = reference;
                    break;
                }
                case CrmAttributeType.DateTime:
                    minValue = DateTime.MinValue;
                    break;

                case CrmAttributeType.Decimal:
                    minValue = 0M;
                    break;

                case CrmAttributeType.Double:
                    minValue = 0.0;
                    break;

                case CrmAttributeType.Integer:
                    minValue = 0;
                    break;

                case CrmAttributeType.Money:
                    minValue = new Money();
                    break;

                case CrmAttributeType.Picklist:
                case CrmAttributeType.State:
                case CrmAttributeType.Status:
                    minValue = new OptionSetValue();
                    break;

                case CrmAttributeType.UniqueIdentifier:
                    minValue = Guid.Empty;
                    break;

                case CrmAttributeType.BigInt:
                    minValue = 0L;
                    break;

                default:
                    minValue = value;
                    break;
            }
            this.Add(new KeyValuePair<string, object>(name, minValue));
            ICrmAttribute attribute = this.crmAttributeAdapterFactory.Create(name, minValue);
            attribute.SetValue(value, data);
            return attribute;
        }

        public IEnumerator<KeyValuePair<string, ICrmAttribute>> GetEnumerator()
        {
            return
                base.Adaptee.Select(
                    a =>
                        new KeyValuePair<string, ICrmAttribute>(a.Key,
                            this.crmAttributeAdapterFactory.Create(a.Key, a.Value))).GetEnumerator();
        }

        public void SetValue(string attributeName, object value)
        {
            base.Adaptee[attributeName] = value;
        }

        public KeyValuePair<string, object>[] StripSystem()
        {
            KeyValuePair<string, object>[] pairArray =
                base.Adaptee.Where(a => this.systemNames.Contains<string>(a.Key)).ToArray();

            foreach (KeyValuePair<string, object> pair in pairArray)
            {
                base.Adaptee.Remove(pair);
            }
            return pairArray;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public CrmEntityAdapter EntityAdapter
        {
            get
            {
                return this.entityAdapter;
            }
        }

        public ICrmAttribute this[string key]
        {
            get
            {
                if (base.Adaptee == null)
                {
                    return null;
                }
                if (!base.Adaptee.ContainsKey(key))
                {
                    return null;
                }
                return this.crmAttributeAdapterFactory.Create(key, base.Adaptee[key]);
            }
        }
    }
}

