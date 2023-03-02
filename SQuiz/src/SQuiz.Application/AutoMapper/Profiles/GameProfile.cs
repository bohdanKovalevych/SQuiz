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
                .ForMember(x => x.QuestionCount, x =>
                {
                    x.Condition(x => x.Quiz != null);
                    x.MapFrom(x => x.Quiz.Questions.Count);
                });

            CreateMap<RegularGameOptionDto, RegularQuizGame>()
                .IncludeBase<GameOptionDto, QuizGame>()
                .ReverseMap()
                .ForMember(x => x.EndDate, x => x.MapFrom(x => x.DateEnd))
                .ForMember(x => x.StartDate, x => x.MapFrom(x => x.DateStart));

            CreateMap<RealtimeGameOptionDto, RealtimeQuizGame>()
                .IncludeBase<GameOptionDto, QuizGame>()
                .ReverseMap();

            CreateMap<GameQuestionDto, Question>()
               .ReverseMap();

            CreateMap<GameAnswerDto, Answer>()
                .ReverseMap();

            CreateMap<PlayerDto, Player>()
                .ReverseMap();

            CreateMap<StartRegularGameDto, RegularQuizGame>();

            CreateMap<StartRealtimeGameDto, RealtimeQuizGame>();
        }
    }
}
