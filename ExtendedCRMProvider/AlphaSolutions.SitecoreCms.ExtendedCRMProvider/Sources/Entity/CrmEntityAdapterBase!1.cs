using System;
using CRMSecurityProvider.Sources.Attribute;
using CRMSecurityProvider.Sources.Entity;
using CRMSecurityProvider.Sources.Repository;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Entity
{
    internal abstract class CrmEntityAdapterBase<T> : AdapterBase<T>, ICrmEntity
    {
        private ICrmKeyValueAttribute initialState;
        private ICrmKeyValueAttribute initialStatus;
        private readonly EntityRepositoryBase repository;

        protected CrmEntityAdapterBase(EntityRepositoryBase repository, T adaptee) : base(adaptee)
        {
            this.repository = repository;
        }

        protected void AttributeCollectionAdapterInitialized()
        {
            this.initialState = this.State;
            this.initialStatus = this.Status;
        }

        public abstract ICrmAttributeCollection Attributes { get; }

        public Guid Id { get; set; }

        public bool IsStateChanged
        {
            get
            {
                return (this.initialState != this.State);
            }
        }

        public bool IsStatusChanged
        {
            get
            {
                return (this.initialStatus != this.Status);
            }
        }

        public abstract string LogicalName { get; set; }

        public EntityRepositoryBase Repository
        {
            get
            {
                return this.repository;
            }
        }

        public ICrmKeyValueAttribute State
        {
            get
            {
                ICrmKeyValueAttribute attribute;
                if ((this.Attributes != null) && ((attribute = this.Attributes["statecode"] as ICrmKeyValueAttribute) != null))
                {
                    return attribute;
                }
                return null;
            }
        }

        public ICrmKeyValueAttribute Status
        {
            get
            {
                ICrmKeyValueAttribute attribute;
                if ((this.Attributes != null) && ((attribute = this.Attributes["statuscode"] as ICrmKeyValueAttribute) != null))
                {
                    return attribute;
                }
                return null;
            }
        }
    }
}

