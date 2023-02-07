using NSubstitute;
using SQuiz.Application.Quizzes.CreateQuiz;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Dtos.Quiz;

namespace SQuiz.Application.UnitTests.Quizzes.CreateQuiz
{
    public class CreateQuizCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _services;

        public CreateQuizCommandTests(ServiceFixture services)
        {
            _services = services;
        }

        [Fact]
        public async Task CreateQUiz_WithValidEditQuizDto_ReturnsUnit()
        {
            // Assign
            var createQuizCommand = new CreateQuizCommand(new EditQuizDto()
            {
                Description = "description",
                IsPublic = true,
                Name = "name"
            }, "userId");
            var handler = new CreateQuizHandler(_services.GetMapper(), _services.QuizService, _services.QuizContext);

            //Act
            await handler.Handle(createQuizCommand, default);

            //Assert
            _services.QuizContext.Questions.Received(1);
        }
    }
}
