using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.ScoreDistribution
{
    public interface IScoreDistributor
    {
        ScoreDistributorResult GetScore(IComment comment);
    }
}
