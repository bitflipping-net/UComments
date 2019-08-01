using System.Threading.Tasks;
using BitFlipping.UComments.Core.Helpers;
using BitFlipping.UComments.Core.Models;
using BitFlipping.UComments.Core.Services;

namespace BitFlipping.UComments.Core.ScoreDistribution
{
    public class IPAddressScoreDistributor : IScoreDistributor
    {
        private readonly CommentService _commentService;

        public IPAddressScoreDistributor(CommentService commentService)
        {
            _commentService = commentService;
        }

        public ScoreDistributorResult GetScore(IComment comment)
        {
            var ipAdress = System.Net.IPAddress.Parse(comment.IPAddress);
            var isBad = Task.Run(async () => await IPAddressHelper.IsBad(ipAdress)).Result;
            if (isBad)
            {
                return new ScoreDistributorResult()
                {
                    Score = 0.0
                };
            }

            return new ScoreDistributorResult()
            {
                Score = comment.Score
            };
        }
    }
}
