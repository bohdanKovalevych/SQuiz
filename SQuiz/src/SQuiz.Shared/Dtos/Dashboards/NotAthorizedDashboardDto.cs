using SQuiz.Shared.Dtos.Game;

namespace SQuiz.Shared.Dtos.Dashboards
{
    public class NotAthorizedDashboardDto
    {
        public List<RegularGameOptionDto> PopularGames { get; set; } = new List<RegularGameOptionDto>();

        public List<RegularGameOptionDto> NewGames { get; set; } = new List<RegularGameOptionDto>();

        public List<string> PopularUsers { get; set; } = new List<string>();
    }
}
