using AutoMapper;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Models;

namespace SQuiz.Application.AutoMapper.Profiles
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<EditQuizDto, Quiz>()
                .ForMember(x => x.AuthorId, x => x.Ignore())
                .ForMember(x => x.DateCreated, x => x.Ignore())
                .ForMember(x => x.DateUpdated, x => x.Ignore())
                .ForMember(x => x.ShortId, x => x.Ignore())
                .ReverseMap();
            
            CreateMap<Moderator, ModeratorDto>();

            CreateMap<Quiz, QuizDetailsDto>()
                .ForMember(x => x.Moderators, 
                    x => x.MapFrom(x => x.QuizModerators
                        .Select(x => x.Moderator)
                        .ToList()));

            CreateMap<QuestionDto, Question>()
                .ForMember(x => x.QuizId, x => x.Ignore())
                .ForMember(x => x.CorrectAnswer, x => x.Ignore())
                .ReverseMap();

            CreateMap<AnswerDto, Answer>()
                .ForMember(x => x.Question, x => x.Ignore())
                .ReverseMap();
        }
    }
}
