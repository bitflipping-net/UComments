using System.Web.Mvc;
using Umbraco.Web.Mvc;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Web.Controllers
{
    [PluginController(Constants.PluginName)]
    public class CommentsController : SurfaceController
    {
        private readonly CommentService commentService;

        public CommentsController()
        {
            this.commentService = CommentService.Current;
        }

        [ChildActionOnly]
        public ActionResult CommentForm()
        {
            if (Members.IsLoggedIn())
            {
                var model = new MemberCommentModel()
                {
                    MemberId = Members.GetCurrentMemberId(),
                    ContentId = CurrentPage.Id
                };

                return PartialView("_MemberCommentForm", model);
            }
            else
            {
                var model = new CommentModel()
                {
                    ContentId = CurrentPage.Id
                };

                return PartialView("_CommentForm", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleComment(CommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var comment = commentService.CreateCommentByEmail(model.ContentId, model.Name, model.Email, model.BodyText, model.ParentId);
            TempData["CommentSuccess"] = comment;

            return Redirect("#comment-created");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleAuthenticatedComment(MemberCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var comment = commentService.CreateCommentByMember(model.ContentId, model.MemberId, model.BodyText, model.ParentId);
            TempData["CommentSuccess"] = comment;

            //return Redirect("#comment-" + comment.Id);
            return Redirect("#comment-created");
        }
    }
}
