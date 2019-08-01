using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using BitFlipping.UComments.Core.Events;
using BitFlipping.UComments.Core.Helpers;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Persistence.Models;
using BitFlipping.UComments.Core.Repositories;

namespace BitFlipping.UComments.Core.Services
{
    public class CommentService
    {
        private CommentRepository comments;
        private SendService sendService;
        private UmbracoHelper umbracoHelper;

        private static CommentService _current;
        /// <summary>
        /// Get current instance
        /// </summary>
        public static CommentService Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new CommentService(ApplicationContext.Current, UmbracoContext.Current);
                }
                return _current;
            }
        }

        public CommentService(ApplicationContext appContext, UmbracoContext umbracoContext)
        {
            this.comments = new CommentRepository(appContext.DatabaseContext);
            this.sendService = new SendService();
            this.umbracoHelper = new UmbracoHelper(umbracoContext);
        }

        /// <summary>
        /// Create public comment
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="author"></param>
        /// <param name="email"></param>
        /// <param name="bodyText"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IComment CreateCommentByEmail(int contentId, string author, string email, string bodyText, int? parentId)
        {
            var comment = new CommentEntity()
            {
                Id = 0, // NEW
                Key = Guid.NewGuid(),
                ContentId = contentId,
                ParentId = parentId,
                Author = author,
                Email = email,
                BodyText = bodyText,
            };

            return CreateComment(comment);
        }


        /// <summary>
        /// Create member comment
        /// </summary>
        /// <param name="contentId">Umbraco Content Id</param>
        /// <param name="memberId"></param>
        /// <param name="bodyText"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IComment CreateCommentByMember(int contentId, int memberId, string bodyText, int? parentId)
        {
            var comment = new CommentEntity()
            {
                ContentId = contentId,
                ParentId = parentId,
                MemberId = memberId,
                BodyText = bodyText,
                IsApproved = false,
                Deleted = false,
            };

            return CreateComment(comment);
        }

        private IComment CreateComment(CommentEntity comment)
        {
            comment.Id = 0; // NEW
            comment.IPAddress = IPAddressHelper.GetIPAddress().ToString();
            comment.CreateDate = DateTime.UtcNow;
            comment.Deleted = false;
            comment.IsApproved = false; // TODO: Configuration for auto-approval

            var creatingArgs = new CommentEventArgs()
            {
                CommentId = comment.Id,
                Entity = comment
            };
            Creating?.Invoke(this, creatingArgs);

            this.comments.Insert(comment);

            var createdArgs = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment
            };

            Created?.Invoke(this, createdArgs);

            return comment;
        }

        public PagedResult<IComment> GetPagedComments(PagedCommentsQuery query)
        {
            var pagedResult = this.comments.GetPagedComments(query);
            var pagedModel = new PagedResult<IComment>(pagedResult.TotalItems, pagedResult.CurrentPage, pagedResult.ItemsPerPage)
            {
                Items = pagedResult.Items
            };
            return pagedModel;
        }

        public IComment GetComment(int commentId)
        {
            return this.comments.Get(commentId);
        }

        /// <summary>
        /// Get approved comments for content
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public IEnumerable<IComment> GetComments(int contentId)
        {
            var comments = this.comments.GetPagedComments(new PagedCommentsQuery()
            {
                ContentId = contentId,
                Status = CommentStatus.Approved
            });
            return comments.Items;
        }

        public PagedResult<IComment> GetPagedComments(int contentId, long pageNumber, long itemPerPage)
        {
            var pagedResult = this.comments.GetPagedComments(new PagedCommentsQuery()
            {
                ContentId = contentId,
                PageNumber = pageNumber,
                ItemsPerPage = itemPerPage
            });

            var pagedModel = new PagedResult<IComment>(pagedResult.TotalItems, pagedResult.CurrentPage, pagedResult.ItemsPerPage)
            {
                Items = pagedResult.Items
            };

            return pagedModel;
        }

        public void UpdateComment(int id, string bodyText, string author, string email)
        {
            var comment = GetComment(id);
            comment.BodyText = bodyText;
            comment.Author = author;
            comment.Email = email;

            UpdateComment(comment);
        }

        private void UpdateComment(IComment comment)
        {
            var updatingArgs = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment
            };

            Updating?.Invoke(this, updatingArgs);

            var savedComment = new CommentEntity()
            {
                Id = comment.Id,
                Author = comment.Author,
                BodyText = comment.BodyText,
                Email = comment.Email,
                IPAddress = comment.IPAddress,
                IsApproved = comment.IsApproved,
                Score = comment.Score,
                Deleted = comment.Deleted,
                MemberId = comment.MemberId,
                ContentId = comment.ContentId,
                ParentId = comment.ParentId,
                CreateDate = comment.CreateDate,
                UpdateDate = DateTime.UtcNow
            };

            this.comments.Update(savedComment);

            var updatedArgs = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment
            };
            Updated?.Invoke(this, updatedArgs);
        }

        public async Task<IComment> ToggleApprove(int commentId, bool sentApprovedMessage = true)
        {
            var comment = GetComment(commentId);

            comment.IsApproved = true;
            comment.Deleted = false;

            UpdateComment(comment);

            if (sentApprovedMessage)
            {
                // TODO: Change view to something else
                string view = "~/Views/Email/CommentApproved.cshtml";

                var content = this.umbracoHelper.TypedContent(comment.ContentId);

                if (content == null)
                    throw new NullReferenceException("node");

                var mailModel = new BitFlipping.UComments.Web.Models.CommentApprovedMailModel()
                {
                    Author = comment.Author,
                    PermaLink = content.UrlAbsolute()
                };

                await this.sendService.SendEmail(comment.Author, view, mailModel);
            }

            var approvedArgs = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment
            };
            ToggledApproved?.Invoke(this, approvedArgs);

            return comment;
        }

        public void ToggleTrash(int commentId)
        {
            var comment = GetComment(commentId);

            comment.Deleted = !comment.Deleted;
            comment.IsApproved = false;
            UpdateComment(comment);

            var args = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment

            };
            ToggledTrashed?.Invoke(this, args);
        }

        public void ToggleSpam(int commentId)
        {
            var comment = this.GetComment(commentId);
            comment.Score = comment.Score > 0.0 ? 0.0 : 1.0; // Is spam
            UpdateComment(comment);

            var args = new CommentEventArgs
            {
                CommentId = comment.Id,
                Entity = comment
            };
            ToggledMarkedAsSpam?.Invoke(this, args);
        }

        /// <summary>
        /// Hard-delete comment from database
        /// </summary>
        /// <param name="comment"></param>
        private void DeleteComment(CommentEntity comment)
        {
            this.comments.Delete(comment);

            var deletedArgs = new CommentEventArgs
            {
                CommentId = comment.Id
            };

            Deleted?.Invoke(this, deletedArgs);
        }

        public static event CommentEventHandler Creating;
        public static event CommentEventHandler Created;
        public static event CommentEventHandler ToggledApproved;
        public static event CommentEventHandler ToggledMarkedAsSpam;
        public static event CommentEventHandler ToggledTrashed;
        public static event CommentEventHandler Deleted;
        public static event CommentEventHandler Updating;
        public static event CommentEventHandler Updated;

        public delegate void CommentEventHandler(object sender, CommentEventArgs args);
    }
}
