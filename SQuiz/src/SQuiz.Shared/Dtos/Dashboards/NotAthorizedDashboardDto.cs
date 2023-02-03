using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Dtos.Dashboards
{
    public class NotAthorizedDashboardDto
    {
        public List<GameOptionDto> PopularGames { get; set; } = new List<GameOptionDto>();

        public List<GameOptionDto> NewGames { get; set; } = new List<GameOptionDto>();

        public List<string> PopularUsers { get; set; } = new List<string>();
    }
}
