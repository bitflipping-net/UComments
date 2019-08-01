using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.Models
{
    public class PagedCommentsQuery
    {
        public long? PageNumber { get; set; }
        public long? ItemsPerPage { get; set; }
        public int? ContentId { get; set; }
        public CommentStatus Status { get; set; }
        public int? MaxLevel { get; set; }
        public PagedCommentsQueryOrdering OrderBy { get; set; }
        public string SearchTerm { get; set; }
    }

    public enum PagedCommentsQueryOrdering
    {
        Path,
        PathDesc,
        CreateDate,
        CreateDateDesc,
    }
}
