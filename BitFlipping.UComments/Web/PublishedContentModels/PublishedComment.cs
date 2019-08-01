using System;
using Umbraco.Core.Models;
using Umbraco.Web;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Web.Models
{
    public class PublishedComment : IComment
    {
        public PublishedComment()
        {

        }

        public PublishedComment(IComment comment, IPublishedContent content)
        {
            Id = comment.Id;
            ContentId = comment.ContentId;
            Page = content;
            IsApproved = comment.IsApproved;
            Author = comment.Author;
            Email = comment.Email;
            BodyText = comment.BodyText;
            CreateDate = comment.CreateDate;
            UpdateDate = comment.UpdateDate;
            Deleted = comment.Deleted;
            Score = comment.Score;
            ParentId = comment.ParentId;
            MemberId = comment.MemberId;
            IPAddress = comment.IPAddress;
            Url = content.UrlAbsolute() + "#comment-" + comment.Key;
        }

        public int Id { get; set; }

        public Guid Key { get; set; }

        public int ContentId { get; set; }

        public IPublishedContent Page { get; set; }

        public bool IsApproved { get; set; }

        public string Author { get; set; }

        public string Email { get; set; }

        public string BodyText { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Deleted { get; set; }

        public double Score { get; set; }

        public int? ParentId { get; set; }

        public int? MemberId { get; set; }

        public DateTime UpdateDate { get; set; }

        public string IPAddress { get; set; }

        public string Url { get; set; }
    }
}
