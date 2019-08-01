using System.Net.Http;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Web.Controllers
{
    [PluginController(Constants.PluginName)]
    public class CommentApiController : UmbracoApiController
    {
        private readonly CommentService commentService;

        public CommentApiController()
        {
            this.commentService = new CommentService(ApplicationContext, UmbracoContext);
        }

        [HttpGet]
        public HttpResponseMessage GetComments(int pageId, long pageNumber = 1, long pageSize = 20, int maxLevel = 2)
        {
            var comments = this.commentService.GetPagedComments(new PagedCommentsQuery() {
                ContentId = pageId,
                PageNumber = pageNumber,
                ItemsPerPage = pageSize,
                MaxLevel = maxLevel
            });
            return Request.CreateResponse(comments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage CreateComment(CommentModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateValidationErrorResponse(ModelState);

            var comment = this.commentService.CreateCommentByEmail(
                model.ContentId, 
                model.Name, 
                model.Email, 
                model.BodyText, 
                model.ParentId 
            );

            return Request.CreateResponse(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage CreateMemberComment(MemberCommentModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateValidationErrorResponse(ModelState);

            var comment = this.commentService.CreateCommentByMember(
                model.ContentId,
                model.MemberId,
                model.BodyText,
                model.ParentId
            );

            return Request.CreateResponse(comment);
        }
    }
}
