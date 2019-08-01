using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BitFlipping.UComments.Core.Backoffice.Models
{
    public class CommentModel : Core.Models.IComment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public Guid Key { get; set; }

        [Required]
        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("bodyText")]
        public string BodyText { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("isTrashed")]
        public bool Deleted { get; set; }

        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        [JsonProperty("parent")]
        public CommentModel Parent { get; set; }

        [JsonProperty("memberId")]
        public int? MemberId { get; set; }

        [JsonProperty("member")]
        public MemberModel Member { get; set; }

        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("updateDate")]
        public DateTime UpdateDate { get; set; }

        [JsonProperty("ipAddress")]
        public string IPAddress { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class MemberModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
