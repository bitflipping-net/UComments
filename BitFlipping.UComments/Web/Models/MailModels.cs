using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitFlipping.UComments.Web.Models;

namespace BitFlipping.UComments.Core.Models
{
    public class CommentReplyMailModel
    {
        public PublishedComment Reply { get; set; }
        public PublishedComment ReplyTo { get; set; }
    }

    public class CommentApprovedMailModel
    {
        public PublishedComment Comment { get; set; }
    }

    public class MailButton
    {
        public string Url { get; set; }
        public string Label { get; set; }
    }
}
