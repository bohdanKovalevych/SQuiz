using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Models;

namespace SQuiz.Shared.Services
{
    public class PointsCounter : IPointsCounter
    {
        public int GetPoints(TimeSpan timeToCount, Question.ANSWERING_TIME time, Question.POINTS points)
        {
            var bonusMultiplier = points switch
            {
                Question.POINTS.Double => 2000,
                Question.POINTS.Normal or _ => 1000,
            };
            
            var maxTime = time switch
            {
                Question.ANSWERING_TIME.Short => TimeSpan.FromSeconds(45),
                Question.ANSWERING_TIME.Long or _ => TimeSpan.FromSeconds(90),
            };

            var decisionTime = maxTime > timeToCount ? timeToCount : maxTime;

            var result = decisionTime / maxTime * bonusMultiplier;
            var normalizedResult = (int)Math.Floor(result);
            
            return normalizedResult;
        }
    }
}
