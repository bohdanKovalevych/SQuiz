using AutoMapper;
using NSubstitute;
using SQuiz.Application.AutoMapper.Profiles;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Interfaces;

namespace SQuiz.Application.UnitTests.Helpers
{
    public class ServiceFixture
    {
        public ServiceFixture()
        {
            QuizService = Substitute.For<IQuizService>();
            QuizContext = Substitute.For<ISQuizContext>();
            PlayGameService = Substitute.For<IPlayGameService>();
            PointsCounter = Substitute.For<IPointsCounter>();
        }

        public IQuizService QuizService { get; }
        public ISQuizContext QuizContext { get; }
        public IPlayGameService PlayGameService { get; }
        public IPointsCounter PointsCounter { get; }

        public IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GameProfile());
                mc.AddProfile(new QuizProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}
