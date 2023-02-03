using NSubstitute;
using SQuiz.Application.Games.ResetPoints;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Games.ResetPoints
{
    public class ResetPointsCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly ResetPointsCommandHandler _handler;

        public ResetPointsCommandTests(ServiceFixture service)
        {
            _service = service;
            _handler = new ResetPointsCommandHandler(_service.QuizContext);
        }

        [Fact]
        public async Task Handle_ShouldResetPlayerPoints()
        {
            // Arrange
            var player = new Player
            {
                Id = "player-id",
                Points = 10,
                PlayerAnswers = new[]
                {
                    new PlayerAnswer(),
                    new PlayerAnswer()
                }
            };
            var playersDbSet = DbSetMockFactory.GetDbSetAsyncMock(new[] { player });
            _service.QuizContext.Players.Returns(playersDbSet);

            // Act
            await _handler.Handle(new ResetPointsCommand("player-id"), CancellationToken.None);

            // Assert
            await _service.QuizContext.Received().SaveChangesAsync(CancellationToken.None);
            Assert.Equal(0, player.Points);
            _service.QuizContext.PlayerAnswers.Received().RemoveRange(player.PlayerAnswers);
        }

        [Fact]
        public async Task Handle_ShouldDoNothing_WhenPlayerIsNotFound()
        {
            // Arrange
            var playersDbSet = DbSetMockFactory.GetDbSetAsyncMock(Array.Empty<Player>());
            _service.QuizContext.Players.Returns(playersDbSet);

            // Act
            await _handler.Handle(new ResetPointsCommand("player-id"), CancellationToken.None);

            // Assert
            await _service.QuizContext.DidNotReceive().SaveChangesAsync(CancellationToken.None);
        }
    }
}
