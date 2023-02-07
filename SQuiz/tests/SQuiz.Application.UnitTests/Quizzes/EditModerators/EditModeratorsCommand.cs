using NSubstitute;
using SQuiz.Application.Quizzes.EditModerators;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Quizzes.EditModerators
{
    public class EditModeratorsCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly EditModeratorsCommandHandler _handler;

        public EditModeratorsCommandTests(ServiceFixture service)
        {
            _service = service;
            _handler = new EditModeratorsCommandHandler(_service.QuizContext);
        }

        [Fact]
        public async Task Handle_WhenQuizIsNotFound_ShouldReturnNotFoundResult()
        {
            // Arrange
            var command = new EditModeratorsCommand(new EditQuizDto
            {
                Id = Guid.NewGuid().ToString(),
                Moderators = new List<ModeratorDto>()
                {
                    new ModeratorDto()
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            });
            var dbSet = DbSetMockFactory.GetDbSetAsyncMock(Array.Empty<Quiz>());
            _service.QuizContext.Quizzes.Returns(dbSet);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Asser
            Assert.False(result.IsSuccess);
            result.IfFail(x => Assert.IsType<NotFoundException>(x));
        }

        [Fact]
        public async Task Handle_WhenQuizIsFound_RemovesAndAddsModerators()
        {
            // Arrange
            var quizId = Guid.NewGuid().ToString();
            var model = new EditQuizDto
            {
                Id = quizId,
                Moderators = new List<ModeratorDto>()
                {
                    new ModeratorDto { Id = "m1" },
                    new ModeratorDto { Id = "m2" }
                }
            };

            var quiz = new Quiz
            {
                Id = quizId,
                QuizModerators = new[]
                {
                    new QuizModerator { Id = Guid.NewGuid().ToString(), ModeratorId = Guid.NewGuid().ToString() },
                    new QuizModerator { Id = Guid.NewGuid().ToString(), ModeratorId = model.Moderators[0].Id },
                    new QuizModerator { Id = Guid.NewGuid().ToString(), ModeratorId = Guid.NewGuid().ToString() }
                }
            };

            var dbSet = DbSetMockFactory.GetDbSetAsyncMock(new List<Quiz> { quiz });
            _service.QuizContext.Quizzes.Returns(dbSet);

            // Act
            var result = await _handler.Handle(new EditModeratorsCommand(model), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            await _service.QuizContext.Received().SaveChangesAsync(CancellationToken.None);
        }
    }
}
