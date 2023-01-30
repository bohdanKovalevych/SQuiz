namespace SQuiz.Shared.Dtos.Game
{
    public class ReceivedPointsDto
    {
        public string CorrectAnswerId { get; set; }
        public string? SelectedAnswerId { get; set; }
        public int CurrentPoints { get; set; }
        public int TotalPoints { get; set; }
    }
}
