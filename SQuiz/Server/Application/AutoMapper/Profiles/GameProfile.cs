using AutoMapper;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Server.Application.AutoMapper.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<GameQuestionDto, Question>()
               .ReverseMap();

            CreateMap<GameAnswerDto, Answer>()
                .ReverseMap();

            CreateMap<PlayerDto, Player>()
                .ReverseMap();
        }
    }
}
