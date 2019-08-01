using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitFlipping.UComments.Core.Models
{
    public enum CommentStatus
    {
        Any,
        Pending,
        Approved,
        MarkedAsSpam,
        Trashed
    }
}
