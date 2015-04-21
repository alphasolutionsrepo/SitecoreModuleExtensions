using System;
using CRMSecurityProvider.Caching;
using CRMSecurityProvider.crm3.webservice;
using CRMSecurityProvider.Repository;
using CRMSecurityProvider.Repository.V3;
using CRMSecurityProvider.Utils;
using Sitecore.Diagnostics;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Repository.V3
{
    /// <summary>
    /// Not Used at this point
    /// </summary>
    class RoleRepositoryV3 : CRMSecurityProvider.Repository.V3.RoleRepositoryV3
    {
        public RoleRepositoryV3(ICrmServiceV3 crmService, IMarketingListToRoleConverterV3 marketingListToRoleConverter, IContactToUserConverterV3 contactToUserConverter, UserRepositoryBase userRepository, ICacheService cacheService)  : base(crmService, marketingListToRoleConverter, contactToUserConverter, userRepository, cacheService)
        {
        }

        public override string[] GetUsersInRole(string roleName)
        {
            Assert.ArgumentNotNull(roleName, "roleName");
            ConditionalLog.Info(string.Format("GetUsersInRole({0}). Started.", roleName), this, TimerAction.Start, "getUsersInRole");
            string text = base.CacheService.MembersCache.Get(roleName);
            if (text != null)
            {
                ConditionalLog.Info(string.Format("GetUsersInRole({0}). Finished (users have been retrieved from cache).", roleName), this, TimerAction.Stop, "getUsersInRole");
                return text.Split(new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
            }

            // set up a query for the list to check type
            ColumnSet columnSet = new ColumnSet();
            columnSet.Attributes = new string[]
	        {	"listname", "query", "type"};

            QueryExpression query = new QueryExpression();
            query.ColumnSet = columnSet;

            ConditionExpression listnameCondition = new ConditionExpression();
            listnameCondition.AttributeName = "listname";
            listnameCondition.Operator = ConditionOperator.Equal;
            listnameCondition.Values = new String[] { roleName };

            FilterExpression filterList = new FilterExpression();
            filterList.Conditions = new ConditionExpression[]
	        {
		        listnameCondition
            };  
            filterList.FilterOperator = LogicalOperator.And;
            query.EntityName = "list";
            query.Criteria = filterList;

            // Execute the query
             RetrieveMultipleRequest req = new RetrieveMultipleRequest();
             req.Query = query;
             req.ReturnDynamicEntities = true;

             RetrieveMultipleResponse res
                = (RetrieveMultipleResponse)CrmService.Execute(req);

             if (res.BusinessEntityCollection.BusinessEntities.GetLength(0) > 0)
             {
                 DynamicEntity myList = (DynamicEntity)res.BusinessEntityCollection.BusinessEntities[0];

                 
             }
             return base.GetUsersInRole(roleName);
             
        }
    }
}
