using System.Collections.Generic;
using Umbraco.Core;
using BitFlipping.UComments.Core.Events;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Core.ScoreDistribution
{
    public class ScoreDistributionEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            CommentService.Creating += Creating;
        }

        private void Creating(object sender, CommentEventArgs args)
        {
            var commentService = (CommentService)sender;
            IComment comment = args.Entity;

            var scoreDistributorContext = new ScoreDistributionContext();
            IEnumerable<IScoreDistributorResult> results = scoreDistributorContext.GetResultsForComment(comment);
            comment.Score = scoreDistributorContext.GetScore(results);
        }
    }
}
