using Examine.LuceneEngine;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Core.ExamineIndexes
{
    public class CommentDataService : ISimpleDataService
    {
        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            var commentService = new CommentService(ApplicationContext.Current, UmbracoContext.Current);

            var pagedComments = commentService.GetPagedComments(new Models.PagedCommentsQuery());

            return Enumerable.Empty<SimpleDataSet>();
        }
    }
}
