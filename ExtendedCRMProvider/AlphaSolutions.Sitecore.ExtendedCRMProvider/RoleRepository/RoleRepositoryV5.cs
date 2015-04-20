using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

using CRMSecurityProvider.Caching;
using CRMSecurityProvider.Utils;

using Sitecore.Diagnostics;
using CRMSecurityProvider.Repository.V5;
using CRMSecurityProvider.Repository;
using Microsoft.Xrm.Sdk.Query;
using CRMSecurityProvider.Configuration;
using System.IO;
using System.Xml;

namespace AlphaSolutions.Sitecore.ExtendedCRMProvider.RoleRepository
{
    class RoleRepositoryV5 : CRMSecurityProvider.Repository.V5.RoleRepositoryV5
    {
        public RoleRepositoryV5(IOrganizationService crmService, IMarketingListToRoleConverterV5 marketingListToRoleConverter, IContactToUserConverterV5 contactToUserConverter, UserRepositoryBase userRepository, ICacheService cacheService)
            : base(crmService, marketingListToRoleConverter, contactToUserConverter, userRepository, cacheService)
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
                return text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // set up a query for the list to check type
            Microsoft.Xrm.Sdk.Query.ColumnSet columnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(new string[] { "listname", "query", "type" });

            Microsoft.Xrm.Sdk.Query.QueryExpression query = new Microsoft.Xrm.Sdk.Query.QueryExpression();
            query.ColumnSet = columnSet;

            Microsoft.Xrm.Sdk.Query.ConditionExpression listnameCondition = new Microsoft.Xrm.Sdk.Query.ConditionExpression();
            listnameCondition.AttributeName = "listname";
            listnameCondition.Operator = Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal;
            listnameCondition.Values.Add(roleName);

            Microsoft.Xrm.Sdk.Query.FilterExpression filterList = new Microsoft.Xrm.Sdk.Query.FilterExpression();
            filterList.Conditions.Add(listnameCondition);
            filterList.FilterOperator = Microsoft.Xrm.Sdk.Query.LogicalOperator.And;
            query.EntityName = "list";
            query.Criteria = filterList;

            // Execute the query
            Microsoft.Xrm.Sdk.Messages.RetrieveMultipleRequest req = new Microsoft.Xrm.Sdk.Messages.RetrieveMultipleRequest();
            req.Query = query;

            Microsoft.Xrm.Sdk.Messages.RetrieveMultipleResponse res
               = (Microsoft.Xrm.Sdk.Messages.RetrieveMultipleResponse)this.OrganizationService.Execute(req);

            if (res != null && res.EntityCollection != null && res.EntityCollection.Entities.Count > 0)
            {
                Entity myList = res.EntityCollection.Entities[0];
                if (myList.Attributes.Keys.Contains("query"))
                {
                    // Define the fetch attributes.
                    // Set the number of records per page to retrieve.
                    int fetchCount = Settings.FetchThrottlingPageSize;
                    // Initialize the page number.
                    int pageNumber = 1;
                    // Initialize the number of records.
                    int recordCount = 0;
                    // Specify the current paging cookie. For retrieving the first page, 
                    // pagingCookie should be null.
                    string pagingCookie = null;

                    //Convert fetchXML to Query Expression
                    var xml = myList["query"].ToString();
                    
                    HashSet<string> hashSet = new HashSet<string>();


                    try
                    {
                        while (true)
                        {
                            string xmlpaging = CreateXml(xml, pagingCookie, pageNumber, fetchCount, Settings.UniqueKeyProperty);
                            Microsoft.Xrm.Sdk.Query.FetchExpression f = new Microsoft.Xrm.Sdk.Query.FetchExpression(xmlpaging);
                            RetrieveMultipleRequest retrieveMultipleRequest = new RetrieveMultipleRequest();
                            retrieveMultipleRequest.Query = f; 
                            RetrieveMultipleResponse retrieveMultipleResponse = (RetrieveMultipleResponse)this.OrganizationService.Execute(retrieveMultipleRequest);
                            if (retrieveMultipleResponse != null && retrieveMultipleResponse.EntityCollection != null)
                            {
                                ConditionalLog.Info(string.Format("GetUsersInRole({0}). Retrieved {1} users from CRM.", roleName, retrieveMultipleResponse.EntityCollection.Entities.Count), this, TimerAction.Tick, "getUsersInRole");
                                foreach (Entity current in retrieveMultipleResponse.EntityCollection.Entities)
                                {
                                    try
                                    {
                                        base.CacheService.UserCache.Add(this.ContactToUserConverter.Convert(current));
                                        hashSet.Add((string)current[Settings.UniqueKeyProperty]);
                                    }
                                    catch (Exception e)
                                    {
                                        ConditionalLog.Error(string.Format("GetUsersInRole({0}). Error in converting contact to user. Number of attributes gotten: {1}", current.LogicalName, current.Attributes.Count),e,this);
                                    }
                                }
                                // Check for morerecords, if it returns 1.
                                if (retrieveMultipleResponse.EntityCollection.MoreRecords)
                                {
                                    // Increment the page number to retrieve the next page.
                                    pageNumber++;
                                }
                                else
                                {
                                    // If no more records in the result nodes, exit the loop.
                                    break;
                                } 
                                pagingCookie = retrieveMultipleResponse.EntityCollection.PagingCookie;
                            }
                        }
                        var ret = hashSet.ToArray<string>();
                        base.CacheService.MembersCache.Add(roleName, string.Join("|", ret));
                        return ret;
                    }
                    catch (System.Exception sourceException)
                    {
                        ConditionalLog.Error(string.Format("Couldn't get contacts of {0} marketing list from CRM.", roleName), sourceException, this);
                    }

                }
                else
                {
                    return base.GetUsersInRole(roleName);
                }
            }
            return new string[0];
        }

        protected string CreateXml(string xml, string cookie, int page, int count, string returnattribute)
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(stringReader);

            // Load document
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            return CreateXml(doc, cookie, page, count, returnattribute);
        }

        protected string CreateXml(XmlDocument doc, string cookie, int page, int count, string returnattribute)
        {
            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);
            XmlNode n = doc.SelectSingleNode("//attribute");
            XmlNode id = n.CloneNode(false);
            n.ParentNode.AppendChild(id);
            id.Attributes["name"].Value = returnattribute;

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}
