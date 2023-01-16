using SQuiz.Shared.Models;

namespace SQuiz.Shared.Interfaces
{
    public interface IPointsCounter
    {
        public int GetPoints(TimeSpan timeToCount, Question question);
    }
}
