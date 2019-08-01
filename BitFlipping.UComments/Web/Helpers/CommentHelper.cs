using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using BitFlipping.UComments.Core.Services;
using BitFlipping.UComments.Web.Models;

namespace BitFlipping.UComments.Web.Helpers
{
    public class CommentHelper
    {
        private static CommentHelper _current;
        private CommentService commentService;
        private UmbracoHelper umbracoHelper;

        public CommentHelper()
        {
            this.commentService = CommentService.Current;
            this.umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        }

        public CommentHelper(CommentService commentService, UmbracoContext umbracoContext)
        {

        }

        public static CommentHelper Current
        {
            get
            {
                if(_current == null)
                {
                    _current = new CommentHelper();
                }
                return _current;
            }
        }

        private double? _scoreThreshold;
        public double GetScoreThreshold()
        {
            if (!_scoreThreshold.HasValue)
            {
                string strThreshold = ConfigurationManager.AppSettings["uComments.ScoreThreshold"] as string;
                if (!string.IsNullOrEmpty(strThreshold))
                {
                    if (double.TryParse(strThreshold, out double scoreThreshold))
                    {
                        if(scoreThreshold <= 0.0)
                        {
                            throw new InvalidOperationException("ScoreThreshold must be greater than 0.0");
                        }

                        if (scoreThreshold >= 1.0)
                        {
                            throw new InvalidOperationException("ScoreThreshold must be less than 1.0");
                        }

                        _scoreThreshold = scoreThreshold;
                    }
                }

                _scoreThreshold = 5.0;
            }
            return _scoreThreshold.Value;
        }

        public IEnumerable<PublishedComment> GetComments(int pageId)
        {
            return this.commentService.GetComments(pageId)
                .Select(c => new PublishedComment(c, GetPublishedPage(pageId)));
        }

        public IEnumerable<PublishedComment> GetComments(int pageId, long pageNumber, long pageSize)
        {
            return this.commentService.GetComments(pageId)
                .Select(c => new PublishedComment(c, GetPublishedPage(pageId)));
        }

        public IPublishedContent GetPublishedPage(int pageId)
        {
            return this.umbracoHelper.TypedContent(pageId);
        }
    }
}
