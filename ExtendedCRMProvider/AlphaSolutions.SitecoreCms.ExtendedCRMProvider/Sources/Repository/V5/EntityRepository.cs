using System;
using System.Collections.Generic;
using System.Linq;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Attribute.Metadata;
using AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5.Entity;
using CRMSecurityProvider.Caching;
using CRMSecurityProvider.Sources.Attribute;
using CRMSecurityProvider.Sources.Attribute.Metadata;
using CRMSecurityProvider.Sources.Entity;
using CRMSecurityProvider.Sources.PagingInfo;
using CRMSecurityProvider.Sources.Repository;
using CRMSecurityProvider.Sources.Repository.V5.Extensions;
using CRMSecurityProvider.Utils;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Sitecore.StringExtensions;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Sources.Repository.V5
{
    public class EntityRepository : EntityRepositoryBase
    {
        private readonly IOrganizationService _organizationService;

        public EntityRepository(IOrganizationService organizationService, ICacheService cacheService)
            : base(cacheService)
        {
            this._organizationService = organizationService;
        }

        public override void Delete(string entityName, Guid id)
        {
            this._organizationService.Delete(entityName, id);
        }

        public override CrmEntityMetadata[] GetAllEntitiesMetadata()
        {
            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.Default
            };
            RetrieveAllEntitiesResponse response =
                (RetrieveAllEntitiesResponse) this._organizationService.Execute(request);

            return response.EntityMetadata.Select(e => new CrmEntityMetadataAdapter(e)).Cast<CrmEntityMetadata>().ToArray();
        }

        public override ICrmAttributeMetadata GetAttributeMetadata(string entityName, string attributeName)
        {
            CrmAttributeMetadataFactory factory = new CrmAttributeMetadataFactory();
            RetrieveAttributeRequest request = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse response = (RetrieveAttributeResponse) this._organizationService.Execute(request);
            return factory.Create(response.AttributeMetadata);
        }

        public override ICrmEntity[] GetEntities(string logicalName, CrmPagingInfo pagingInfo,
            CrmOrderExpression[] orderExpressions, bool onlyActive)
        {
            FilterExpression expression = new FilterExpression
            {
                FilterOperator = LogicalOperator.And
            };
            if (onlyActive)
            {
                expression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal,
                    new object[] {"Active"}));
            }
            QueryExpression query = new QueryExpression
            {
                EntityName = logicalName,
                ColumnSet = new ColumnSet(true),
                PageInfo = (PagingInfo) pagingInfo,
                Criteria = expression
            };
            query.Orders.AddRange(orderExpressions.Cast<OrderExpression>());
            return
                this._organizationService.RetrieveMultiple(query)
                    .Entities.Select(e => new CrmEntityAdapter(this, e)).Cast<ICrmEntity>().ToArray();
                    
            //        Select<Entity, CrmEntityAdapter>(new Func<Entity, CrmEntityAdapter>(this, (IntPtr) this. <
            //                                                   GetEntities > b__7)).
            //ToArray<CrmEntityAdapter>();
        }

        public override int GetEntitiesCount(string logicalName, string primaryKey, bool onlyActive)
        {
            FilterExpression filter = new FilterExpression
            {
                FilterOperator = LogicalOperator.And
            };
            if (onlyActive)
            {
                filter.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal,
                    new object[] {"Active"}));
            }
            int implicitEntitiesCount = 0;
            try
            {
                FetchExpression query =
                    new FetchExpression(
                        "<fetch mapping='logical' aggregate='true'><entity name='{0}'><attribute name='{1}' aggregate='count' alias='count' />{2}</entity></fetch>"
                            .FormatWith(new object[] {logicalName, primaryKey, filter.ToFetchXml()}));
                EntityCollection entitys = this._organizationService.RetrieveMultiple(query);
                if (entitys.Entities.Count == 0)
                {
                    return 0;
                }
                var entity = entitys.Entities[0];
                implicitEntitiesCount = (int) ((AliasedValue) entity["count"]).Value;
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("AggregateQueryRecordLimit"))
                {
                    implicitEntitiesCount = this.GetImplicitEntitiesCount(logicalName, filter);
                }
            }
            return implicitEntitiesCount;
        }

        public override ICrmEntity GetEntity(string logicalName, string fieldName, string value, bool onlyActive,
            string[] columns)
        {
            FilterExpression expression = new FilterExpression
            {
                FilterOperator = LogicalOperator.And
            };
            expression.AddCondition(new ConditionExpression(fieldName, ConditionOperator.Equal, new object[] {value}));
            if (onlyActive)
            {
                expression.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal,
                    new object[] {"Active"}));
            }
            RetrieveMultipleRequest request2 = new RetrieveMultipleRequest();
            QueryExpression expression3 = new QueryExpression
            {
                ColumnSet = (columns == null) ? new ColumnSet(true) : new ColumnSet(columns),
                EntityName = logicalName
            };
            PagingInfo info = new PagingInfo
            {
                Count = 1,
                PageNumber = 1
            };
            expression3.PageInfo = info;
            expression3.Criteria = expression;
            request2.Query = expression3;
            RetrieveMultipleRequest request = request2;
            RetrieveMultipleResponse response = (RetrieveMultipleResponse) this._organizationService.Execute(request);
            var entity = response.EntityCollection.Entities.FirstOrDefault();
            if (entity == null)
            {
                return null;
            }
            return new CrmEntityAdapter(this, entity);
        }

        public override CrmEntityMetadata GetEntityMetadata(string logicalName)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.All,
                LogicalName = logicalName
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse) this._organizationService.Execute(request);
            return new CrmEntityMetadataAdapter(response.EntityMetadata);
        }

        private int GetImplicitEntitiesCount(string logicalName, FilterExpression filterExpression)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = logicalName,
                Criteria = filterExpression
            };
            PagingInfo info = new PagingInfo
            {
                PageNumber = 1,
                Count = 1,
                ReturnTotalRecordCount = true
            };
            query.PageInfo = info;
            EntityCollection entitys = this._organizationService.RetrieveMultiple(query);
            if (entitys.TotalRecordCountLimitExceeded)
            {
                ConditionalLog.Error(
                    string.Format(
                        "The number of '{0}' entities is exceeding AggregrateQueryRecordLimit. Only first {1} can be shown.",
                        logicalName, entitys.TotalRecordCount), this);
            }
            return entitys.TotalRecordCount;
        }

        public override Guid Insert(ICrmEntity crmEntity)
        {
            CrmEntityAdapter adapter = crmEntity as CrmEntityAdapter;
            if (adapter == null)
            {
                return Guid.Empty;
            }
            KeyValuePair<string, object>[] attributes = adapter.AttributeCollectionAdapter.StripSystem();
            Guid id = adapter.Id = this._organizationService.Create(adapter.Adaptee);
            adapter.AttributeCollectionAdapter.AddRange(attributes);
            if (adapter.IsStateChanged || adapter.IsStatusChanged)
            {
                this.UpdateStateStatus(adapter.LogicalName, id, adapter.State, adapter.Status);
            }
            return id;
        }

        public override ICrmEntity NewEntity(string logicalName)
        {
            return new CrmEntityAdapter(this, new Microsoft.Xrm.Sdk.Entity(logicalName));
        }

        public override void Update(ICrmEntity crmEntity)
        {
            CrmEntityAdapter adapter = crmEntity as CrmEntityAdapter;
            if (adapter != null)
            {
                KeyValuePair<string, object>[] attributes = adapter.AttributeCollectionAdapter.StripSystem();
                this._organizationService.Update(adapter.Adaptee);
                adapter.AttributeCollectionAdapter.AddRange(attributes);
                if (adapter.IsStateChanged || adapter.IsStatusChanged)
                {
                    this.UpdateStateStatus(adapter.LogicalName, adapter.Id, adapter.State, adapter.Status);
                }
            }
        }

        private void UpdateStateStatus(string logicalName, Guid id, ICrmKeyValueAttribute state,
            ICrmKeyValueAttribute status)
        {
            try
            {
                SetStateRequest request = new SetStateRequest
                {
                    EntityMoniker = new EntityReference(logicalName, id),
                    State = new OptionSetValue(state.Key),
                    Status = new OptionSetValue(status.Key)
                };
                this._organizationService.Execute(request);
            }
            catch (Exception exception)
            {
                ConditionalLog.Error("Couldn't update entity state and status", exception, this);
                if (exception.Message.Contains("is not a valid status code for state code"))
                {
                    ConditionalLog.Info("Trying to set state only", this);
                    try
                    {
                        SetStateRequest request2 = new SetStateRequest
                        {
                            EntityMoniker = new EntityReference(logicalName, id),
                            State = new OptionSetValue(state.Key),
                            Status = new OptionSetValue(-1)
                        };
                        this._organizationService.Execute(request2);
                    }
                    catch
                    {
                        ConditionalLog.Error("Couldn't update entity state", exception, this);
                    }
                }
            }
        }

    }
}
