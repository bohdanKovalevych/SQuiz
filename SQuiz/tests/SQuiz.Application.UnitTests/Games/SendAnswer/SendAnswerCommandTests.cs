using NSubstitute;
using SQuiz.Application.Games.SendAnswer;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Games.SendAnswer
{
    public class SendAnswerCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly SendAnswerCommandHandler _handler;

        public SendAnswerCommandTests(ServiceFixture service)
        {
            _service = service;
            _handler = new SendAnswerCommandHandler(_service.QuizContext, _service.PointsCounter);
        }

        [Fact]
        public async Task Handle_AnswerIsCorrect_ReturnsExpectedReceivedPoints()
        {
            // Arrange
            string playerId = "playerId";
            var timeToSolve = TimeSpan.FromSeconds(5);
            var answeringTime = Question.ANSWERING_TIME.Short;
            var points = Question.POINTS.Double;
            var correctAnswerId = "1";
            var question = new Question()
            {
                CorrectAnswerId = correctAnswerId,
                Answers = new List<Answer>() { new Answer() { Id = correctAnswerId } },
                AnsweringTime = answeringTime,
                Points = points
            };
            var player = new Player()
            {
                Id = playerId,
                PlayerAnswers = new List<PlayerAnswer>() { new PlayerAnswer() { Points = 2 }, new PlayerAnswer() { Points = 8 } }
            };
            var dbSetQuestions = DbSetMockFactory.GetDbSetAsyncMock(new[] { question });
            _service.QuizContext.Questions.Returns(dbSetQuestions);
            var dbSetPlayers = DbSetMockFactory.GetDbSetAsyncMock(new[] { player });
            _service.QuizContext.Players.Returns(dbSetPlayers);

            _service.PointsCounter.GetPoints(timeToSolve, answeringTime, points).Returns(8);
            var command = new SendAnswerCommand(playerId, new SendAnswerDto() { AnswerId = question.Answers.First().Id, TimeToSolve = timeToSolve });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            result.IfSucc(x =>
            {
                Assert.Equal(question.CorrectAnswerId, x.CorrectAnswerId);
                Assert.Equal(8, x.CurrentPoints);
                Assert.Equal(question.Answers.First().Id, x.SelectedAnswerId);
                Assert.Equal(10, x.TotalPoints);
            });
        }

        [Fact]
        public async Task Handle_QuestionNotFound_ReturnsBadRequestException()
        {
            // Arrange
            string playerId = Guid.NewGuid().ToString();
            var timeToSolve = TimeSpan.FromSeconds(5);
            var command = new SendAnswerCommand(playerId, new SendAnswerDto { AnswerId = Guid.NewGuid().ToString(), TimeToSolve = timeToSolve });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            result.IfFail(x =>
            {
                Assert.IsType<BadRequestException>(x);
            });
        }
    }
}
