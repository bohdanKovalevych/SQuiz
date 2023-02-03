using NSubstitute;
using SQuiz.Application.Games.GetQuestion;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Games.GetQuestion
{
    public class GetQuestionCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly GetQuestionCommandHandler _handler;

        public GetQuestionCommandTests(ServiceFixture service)
        {
            _service = service;
            _handler = new GetQuestionCommandHandler(_service.QuizContext, _service.GetMapper());
        }

        [Fact]
        public async Task Handle_WhenCalled_InvokesOnStartQuiz()
        {
            // Arrange
            var request = new GetQuestionCommand("player1", 0) { OnStartQuiz = Substitute.For<Func<Task>>() };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            await request.OnStartQuiz.Received().Invoke();
        }

        [Fact]
        public async Task Handle_WhenCalledWithQuestion_InvokesOnIndexChanged()
        {
            // Arrange
            var question = new Question 
            { 
                Order = 1, 
                Quiz = new Quiz()
                {
                    QuizGames = new List<QuizGame>()
                    {
                        new QuizGame()
                        {
                            Players = new List<Player>()
                            {
                                new Player()
                                {
                                    Id = "player1"
                                }
                            }
                        }
                    }
                }
            };

            var questions = new List<Question> { question };
            var dbSetQuestions = DbSetMockFactory.GetDbSetAsyncMock(questions);
            _service.QuizContext.Questions.Returns(dbSetQuestions);

            var request = new GetQuestionCommand("player1", 1) { OnIndexChanged = Substitute.For<Action<int>>() };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            request.OnIndexChanged.Received().Invoke(2);
        }

        [Fact]
        public async Task Handle_WhenNoMoreQuestions_InvokesOnEndQuiz()
        {
            // Arrange
            var dbSetQuestions = DbSetMockFactory.GetDbSetAsyncMock(Array.Empty<Question>());
            _service.QuizContext.Questions.Returns(dbSetQuestions);
            var request = new GetQuestionCommand("player1", 1) { OnEndQuiz = Substitute.For<Func<Task>>() };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            await request.OnEndQuiz.Received().Invoke();
        }
    }
}
