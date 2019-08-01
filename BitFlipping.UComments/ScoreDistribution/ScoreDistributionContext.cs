using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BitFlipping.UComments.Core.Models;

namespace BitFlipping.UComments.Core.ScoreDistribution
{
    public class ScoreDistributionContext
    {
        public IEnumerable<Type> GetScoreDistributors(params Assembly[] assemblies)
        {
            var type = typeof(IScoreDistributor);

            // Find all score distributors
            var distributorTypes = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            return distributorTypes;
        }

        public IEnumerable<Type> GetScoreDistributors()
        {
            return GetScoreDistributors(AppDomain.CurrentDomain.GetAssemblies());
        }

        public IEnumerable<IScoreDistributorResult> GetResultsForComment(IComment comment)
        {
            var distributorTypes = GetScoreDistributors();
            var results = new List<IScoreDistributorResult>();
            foreach (Type distributorType in distributorTypes)
            {
                IScoreDistributor scoreDistributor = (IScoreDistributor)Activator.CreateInstance(distributorType);
                results.Add(scoreDistributor.GetScore(comment));
            }

            return results;
        }

        public double GetScore(IEnumerable<IScoreDistributorResult> results)
        {
            return results.Sum(x => x.Score);
        }
    }
}
