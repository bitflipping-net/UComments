using System;

namespace BitFlipping.UComments.Core.Models
{
    public interface IComment
    {
        int Id { get; set; }
        Guid Key { get; set; }
        int ContentId { get; set; }
        bool IsApproved { get; set; }
        string Author { get; set; }
        string Email { get; set; }
        string BodyText { get; set; }
        DateTime CreateDate { get; set; }
        bool Deleted { get; set; }
        int? ParentId { get; set; }
        int? MemberId { get; set; }
        DateTime UpdateDate { get; set; }
        string IPAddress { get; set; }
        double Score { get; set; }
    }
}