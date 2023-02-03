using Mediator;
using NSubstitute;
using SQuiz.Application.Games.ComputeAndSavePoints;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Games.ComputeAndSavePoints
{
    public class ComputeAndSavePointsCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly ComputeAndSavePointsCommandHandler _handler;

        public ComputeAndSavePointsCommandTests(ServiceFixture service)
        {
            _service = service;
            _handler = new ComputeAndSavePointsCommandHandler(_service.QuizContext);
        }

        [Fact]
        public async Task Handle_WithFoundPlayer_ReturnsUnit()
        {
            //Arrange
            var command = new ComputeAndSavePointsCommand("1");
            var player = new Player()
            {
                Id = "1",
                Name = "name",
                Points = 0,
                PlayerAnswers = new List<PlayerAnswer>()
                {
                    new PlayerAnswer()
                    {
                        Points = 1
                    },
                    new PlayerAnswer()
                    {
                        Points = 1
                    }
                }
            };

            var dbSet = DbSetMockFactory.GetDbSetAsyncMock(new[] { player });
            _service.QuizContext
                .Players
                .Returns(dbSet);

            //Act
            var unit = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<Unit>(unit);
            Assert.Equal(2, player.Points);
        }
    }
}
