using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Persistence.Models;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Backoffice.Controllers
{
    [PluginController(Constants.PluginName)]
    public class CommentDashboardController : UmbracoAuthorizedJsonController
    {
        private CommentService commentService;
        private SendService sendService;

        public CommentDashboardController()
        {
            this.commentService = new CommentService(ApplicationContext, UmbracoContext);
            this.sendService = new SendService();
        }

        [HttpGet]
        public HttpResponseMessage GetComments(int pageNumber = 1, int pageSize = 20, string searchTerm = null, CommentStatus status = CommentStatus.Any)
        {
            var comments = this.commentService.GetPagedComments(new PagedCommentsQuery() {
                PageNumber = pageNumber,
                ItemsPerPage = pageSize,
                Status = status,
                SearchTerm = searchTerm
            });
            return Request.CreateResponse(comments);
        }

        [HttpGet]
        public HttpResponseMessage GetByPage(int pageId)
        {
            var comments = this.commentService.GetComments(pageId);
            return Request.CreateResponse(comments);
        }

        [HttpGet]
        public HttpResponseMessage GetComment(int id)
        {
            var comment = this.commentService.GetComment(id);
            if (comment == null)
                throw new NullReferenceException("comment");

            return Request.CreateResponse(comment);
        }

        [HttpPost]
        public HttpResponseMessage CreateComment([FromBody]CommentEntity model)
        {
            try
            {
                var comment = this.commentService.CreateCommentByEmail(model.ContentId, model.Author, model.Email, model.BodyText, model.ParentId);
                return Request.CreateResponse(comment);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        public HttpResponseMessage UpdateComment([FromBody]CommentEntity model)
        {
            try
            {
                var comment = this.commentService.GetComment(model.Id);
                if (comment == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to find comment by id \"{model.Id}\"");

                this.commentService.UpdateComment(comment.Id, model.BodyText, model.Author, model.Email);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        public HttpResponseMessage ToggleSpam(int id)
        {
            try
            {
                this.commentService.ToggleSpam(id);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        public HttpResponseMessage ToggleTrash(int id)
        {
            try
            {
                this.commentService.ToggleTrash(id);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> ToggleApprove(int id, bool sentApprovedMessage = true)
        {
            try
            {
                var comment = await this.commentService.ToggleApprove(id, sentApprovedMessage);

                return Request.CreateResponse(comment);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //[HttpDelete]
        //public HttpResponseMessage DeleteComment(int id)
        //{
        //    try
        //    {
        //        this.commentService.DeleteComment(id);

        //        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
        //    }
        //}
    }
}
