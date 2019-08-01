using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.Backoffice.Models
{
    public class CommentsFilter
    {
        [JsonProperty("searchTerm")]
        public string SearchTerm { get; set; }

        [JsonProperty("status")]
        public CommentStatus Status { get; set; }
    }
}
