using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.ScoreDistribution
{
    public class ScoreDistributorResult : IScoreDistributorResult
    {
        public ScoreDistributorResult()
        {

        }

        public ScoreDistributorResult(IComment comment)
        {
            Score = comment.Score;
        }

        public ScoreDistributorResult(double score)
        {
            Score = score;
        }

        public double Score { get; set; }
    }
}
