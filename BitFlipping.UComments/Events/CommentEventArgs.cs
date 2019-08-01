using System;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.Events
{
    public class CommentEventArgs : EventArgs
    {
        public int CommentId { get; internal set; }
        public IComment Entity { get; set; }
    }
}
