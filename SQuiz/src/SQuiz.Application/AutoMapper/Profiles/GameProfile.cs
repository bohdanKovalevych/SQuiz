using AutoMapper;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Application.AutoMapper.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<GameOptionDto, QuizGame>()
                .ReverseMap()
                .ForMember(x => x.Name, x => x.MapFrom(x => x.Quiz.Name))
                .ForMember(x => x.EndDate, x => x.MapFrom(x => x.DateEnd))
                .ForMember(x => x.StartDate, x => x.MapFrom(x => x.DateStart));

            CreateMap<GameQuestionDto, Question>()
               .ReverseMap();

            CreateMap<GameAnswerDto, Answer>()
                .ReverseMap();

            CreateMap<PlayerDto, Player>()
                .ReverseMap();
        }
    }
}
