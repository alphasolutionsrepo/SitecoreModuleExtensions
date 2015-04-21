using System.Globalization;
using CRMSecurityProvider.Sources.Attribute;
using Microsoft.Xrm.Sdk;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute
{
    internal class CrmMoneyAttributeAdapter : CrmAttributeAdapter<Money>, ICrmDecimalAttribute, ICrmAttribute<decimal>, ICrmAttribute
    {
        public CrmMoneyAttributeAdapter(CrmAttributeCollectionAdapter crmAttributeCollection, Money internalAttribute) : base(crmAttributeCollection, internalAttribute)
        {
        }

        public override string GetStringifiedValue()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }

        public override void SetValue(string value, params string[] data)
        {
            decimal num;
            if (decimal.TryParse(value, out num))
            {
                base.Adaptee.Value = num;
            }
        }

        public decimal Value
        {
            get
            {
                return base.Adaptee.Value;
            }
        }
    }
}

