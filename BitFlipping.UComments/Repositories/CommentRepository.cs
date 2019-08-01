using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Persistence.Models;
using BitFlipping.UComments.Web.Helpers;

namespace BitFlipping.UComments.Core.Repositories
{
    public class CommentRepository : GenericRepository<int, CommentEntity>
    {
        public CommentRepository(DatabaseContext context) 
            : base(context)
        {
        }

        public Page<CommentEntity> GetPagedComments(PagedCommentsQuery query)
        {
            var sql = new Sql()
                .Select("*")
                .From<CommentEntity>(this.context.SqlSyntax);

            if (query.ContentId.HasValue)
            {
                sql.Append(new Sql().Where<CommentEntity>(c => c.ContentId == query.ContentId, this.context.SqlSyntax));
            }

            if (query.MaxLevel.HasValue)
            {
                sql.Append(new Sql().Where<CommentEntity>(c => c.Level <= query.MaxLevel, this.context.SqlSyntax));
            }


            double scoreThreshold = CommentHelper.Current.GetScoreThreshold();
            switch (query.Status)
            {
                case CommentStatus.Pending:
                    sql.Where<CommentEntity>(c => c.IsApproved == false && (c.Deleted == false), this.context.SqlSyntax);
                    break;
                case CommentStatus.Approved:
                    sql.Append(new Sql().Where<CommentEntity>(c => c.IsApproved, this.context.SqlSyntax));
                    break;
                case CommentStatus.MarkedAsSpam:
                    sql.Append(new Sql().Where<CommentEntity>(c => c.Score < scoreThreshold, this.context.SqlSyntax));
                    break;
                case CommentStatus.Trashed:
                    sql.Append(new Sql().Where<CommentEntity>(c => c.Deleted, this.context.SqlSyntax));
                    break;
                case CommentStatus.Any:
                default:
                    break;
            }

            switch (query.OrderBy)
            {
                case PagedCommentsQueryOrdering.Path:
                    sql.Append(new Sql().OrderBy<CommentEntity>(c => c.Path, this.context.SqlSyntax));
                    break;
                case PagedCommentsQueryOrdering.PathDesc:
                    sql.Append(new Sql().OrderByDescending<CommentEntity>(c => c.Path, this.context.SqlSyntax));
                    break;
                case PagedCommentsQueryOrdering.CreateDate:
                    sql.Append(new Sql().OrderBy<CommentEntity>(c => c.CreateDate, this.context.SqlSyntax));
                    break;
                case PagedCommentsQueryOrdering.CreateDateDesc:
                    sql.Append(new Sql().OrderByDescending<CommentEntity>(c => c.CreateDate, this.context.SqlSyntax));
                    break;
                default:
                    sql.Append(new Sql().OrderBy<CommentEntity>(c => c.Path, this.context.SqlSyntax));
                    break;
            }

            if (query.ItemsPerPage.HasValue)
            {
                var paged = this.context.Database.Page<CommentEntity>(query.PageNumber ?? 1, query.ItemsPerPage.Value, sql);
                return paged;
            }
            else
            {
                var items = this.context.Database.Fetch<CommentEntity>(sql);
                var itemsCount = items.Count;
                return new Page<CommentEntity>()
                {
                    CurrentPage = 1,
                    TotalPages = 1,
                    ItemsPerPage = itemsCount,
                    TotalItems = itemsCount,
                    Items = items,
                };
            }
        }

        public override void Insert(CommentEntity entity)
        {
            base.Insert(entity);

            // Append path with new comment id
            var parentEntity = Get(entity.ParentId.Value);
            entity.Level = parentEntity.Level + 1;
            entity.Path = parentEntity.Path + "," + entity.Id;

            // Update without setting date
            base.Update(entity);
        }
    }
}
