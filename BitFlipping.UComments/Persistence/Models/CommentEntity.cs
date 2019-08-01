using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.Persistence.Models
{
    [TableName(Constants.TableSurfix + "Comments"),
        ExplicitColumns,
        PrimaryKey("id", autoIncrement = true)]
    public class CommentEntity : IComment
    {
        [Column("id"),
            PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("key"),
            Index(IndexTypes.UniqueNonClustered)]
        public Guid Key { get; set; }

        [Column("contentId")]
        public int ContentId { get; set; }

        [Column("isApproved")]
        public bool IsApproved { get; set; }

        [Column("author"),
            NullSetting(NullSetting = NullSettings.Null)]
        [JsonProperty("author")]
        public string Author { get; set; }

        [Column("email"),
            NullSetting(NullSetting = NullSettings.Null)]
        public string Email { get; set; }

        [Column("bodyText"), SpecialDbType(SpecialDbTypes.NTEXT)]
        public string BodyText { get; set; }

        [Column("createDate")]
        public DateTime CreateDate { get; set; }

        [Column("isTrashed")]
        public bool Deleted { get; set; }

        private double _score;

        [Column("score")]
        public double Score
        {
            get { return _score; }
            set { _score = Math.Min(1.0, Math.Max(value, 0.0)); }
        }

        [Column("parentId"),
            NullSetting(NullSetting = NullSettings.Null),
            ForeignKey(typeof(CommentEntity))]
        public int? ParentId { get; set; }

        [Column("memberId"),
            NullSetting(NullSetting = NullSettings.Null)]
        public int? MemberId { get; set; }

        [Column("ipAddress")]
        public string IPAddress { get; set; }

        [Column("updateDate")]
        public DateTime UpdateDate { get; set; }

        [Column("level")]
        public int Level { get; internal set; }

        [Column("path")]
        public string Path { get; internal set; }

        [Ignore]
        public string EmailHash
        {
            get
            {
                // Create a new instance of the MD5CryptoServiceProvider object.  
                MD5 md5Hasher = MD5.Create();

                // Convert the input string to a byte array and compute the hash.  
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(Email.Trim().ToLower()));

                // Create a new Stringbuilder to collect the bytes  
                // and create a string.  
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string.  
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();  // Return the hexadecimal string. 
            }
        }
    }
}
