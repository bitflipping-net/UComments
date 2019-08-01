using Examine;
using System.Collections.Generic;
using Umbraco.Core;

namespace BaseSite.Web.EventHandlers
{
    public class ExternalIndexerEventHandler : IApplicationEventHandler
    {
        #region Events
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
        #endregion

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"].GatheringNodeData += ExternalIndexerEventHandler_GatheringNodeData; ;
        }

                private void ExternalIndexerEventHandler_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {

            if (e.Fields.ContainsKey("path"))
                e.Fields["searchPath"] = e.Fields["path"].Replace(",", " ");


            if (e.Fields.ContainsKey("postCategories"))
                e.Fields["postCategories"] = e.Fields["postCategories"].Replace(",", " ");

            if (e.Fields.ContainsKey("postTags"))
                e.Fields["postTags"] = e.Fields["postTags"].Replace(",", " ");


        }
    }
}