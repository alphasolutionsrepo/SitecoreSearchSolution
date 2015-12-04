using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SitecoreSearchSolution.Web.SearchPipeline.PrepareQueryPipeline;
using AlphaSolutions.Inveniens.Framework.SOLR.Model.Query;


namespace SSS.Examples.SearchPipeline.PrepareQueryPipeline
{
    /// <summary>
    /// Title: PrepareQueryPipeline processor for adding "Security Filter" on all queries
    /// Author: Roland Villemoes / Alpha Solutions
    /// Version: SSS 3.1 / Sitecore 8.1 rev151003
    /// Dependencies: 
    /// - SitecoreSearchSolution.Web.dll
    /// - AlphaSolutions.Inveniens.Framework.dll
    /// - Sitecore.Kernel.dll
    /// 
    /// Prerequisite: Assuming that documents in Solr has a field "securitygroup_string_mv" - a multivalue string field.
    /// This field holds the names of all security groups set in that item when it was pushed from Sitecore (or another source)
    /// Assuming that this piece of code has access to the current users security groups
    /// Add this to the sss.config file like:
    /// 	<SSSPrepareSearchQuery>
    ///         ....
    ///         ....
    ///         <processor type="SSS.Examples.SearchPipeline.PrepareQueryPipeline.SetSecurityGroupFilter, SSS.Examples.SearchPipeline"/>
    ///     </SSSPrepareSearchQuery>
    /// </summary>
    class SetSecurityGroupFilter : PrepareQueryPipelineBase
    {
        public override void Process(PrepareQueryPipelineArgs args)
        {
            var listofUsersSecurityGroups = new List<string>() { "\"group 1\"", "\"group 2\"", "\"group 3\"" };

            /* Example having OR - user has to be part of ONE of security groups: */
            /* SOLR syntaxs needed: fq=securitygroup_string_mv:("group 1" OR "group 2" ...)    */
            var resultingQueryOrPart = string.Format("securitygroup_string_mv:({0})", string.Join(@" OR ", listofUsersSecurityGroups));
            args.SolrQuery.Filters.Add(new StringQueryFilter(resultingQueryOrPart));

            /* Example having AND - user has to be part of ALL security groups: */
            //foreach (var groupName in listofUsersSecurityGroups)
            //{
            //     args.SolrQuery.Filters.Add(new FieldedQuery("securitygroup_string_mv",groupName));
            //}
        }

    }
}
