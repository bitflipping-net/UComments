using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.ScoreDistribution.Distributors
{
    public class EmailAddressScoreDistributor : IScoreDistributor
    {
        public ScoreDistributorResult GetScore(IComment comment)
        {
            return new ScoreDistributorResult(comment);
        }
    }
}
