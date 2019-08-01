using BitFlipping.UComments.Core.DataAnnotations;

namespace BitFlipping.UComments.Core.Models
{
    /// <summary>
    /// HTTP Post Comment
    /// </summary>
    public class CommentModel
    {
        [UmbracoRequired]
        public int ContentId { get; set; }

        /// <summary>
        /// Name of the commenter
        /// </summary>
        [UmbracoRequired]
        [UmbracoDisplayName("comment_author")]
        public string Name { get; set; }

        /// <summary>
        /// E-mail of the commenter
        /// </summary>
        [UmbracoRequired]
        [UmbracoDisplayName("comment_email")]
        [UmbracoEmailAddress(ResourceName="comment_email_error")]
        public string Email { get; set; }

        /// <summary>
        /// Website url of the commenter
        /// </summary>
        [UmbracoDisplayName("comment_websiteUrl")]
        [UmbracoRegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", ResourceName = "comment_websiteUrl_error")]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// The actual comment text
        /// </summary>
        [UmbracoDisplayName("comment_bodyText")]
        [UmbracoRequired]
        public string BodyText { get; set; }

        /// <summary>
        /// Parent comment id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Notify about comments
        /// </summary>
        public bool FollowUp { get; set; }
    }

    /// <summary>
    /// HTTP Post Authenticated Comment
    /// </summary>
    public class MemberCommentModel
    {
        [UmbracoRequired]
        public int ContentId { get; set; }

        /// <summary>
        /// Set logged in member id
        /// </summary>
        [UmbracoRequired]
        public int MemberId { get; set; }

        /// <summary>
        /// The actual comment text
        /// </summary>
        [UmbracoDisplayName("comment_bodyText")]
        [UmbracoRequired]
        public string BodyText { get; set; }

        /// <summary>
        /// Parent comment id
        /// </summary>
        public int? ParentId { get; set; }

    

        /// <summary>
        /// Notify about comments
        /// </summary>
        public bool FollowUp { get; set; }
    }
}