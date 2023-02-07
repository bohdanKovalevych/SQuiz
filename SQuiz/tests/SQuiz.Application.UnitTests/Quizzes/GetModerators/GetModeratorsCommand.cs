using NSubstitute;
using SQuiz.Application.Quizzes.GetModerators;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Quizzes.GetModerators
{
    public class GetModeratorsCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _servcie;
        private readonly GetModeratorsCommandHandler _handler;

        public GetModeratorsCommandTests(ServiceFixture service)
        {
            _servcie = service;
            _handler = new GetModeratorsCommandHandler(_servcie.QuizContext, _servcie.GetMapper());
        }

        [Fact]
        public async Task Handle_Should_Return_All_Moderators_With_Matching_Name_Or_Email()
        {
            // Arrange
            var moderators = new List<Moderator>()
            {
                new Moderator() { Name = "Moderator1", Email = "moderator1@test.com" },
                new Moderator() { Name = "Moderator2", Email = "moderator2@test.com" },
                new Moderator() { Name = "Moderator3", Email = "moderator3@test.com" }
            };

            var moderatorsDbSet = DbSetMockFactory.GetDbSetAsyncMock(moderators);
            _servcie.QuizContext.Moderators.Returns(moderatorsDbSet);

            // Act
            var result = await _handler.Handle(new GetModeratorsCommand(null), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            result.IfSucc(x => Assert.Equal(3, x.Count));
        }

        [Fact]
        public async Task Handle_Should_Return_All_Moderators_With_Matching_Name_Or_Email_With_Search_Query()
        {
            // Arrange
            var moderators = new List<Moderator>
            {
                new Moderator { Name = "Moderator1", Email = "moderator1@test.com" },
                new Moderator { Name = "Moderator2", Email = "moderator2@test.com" },
                new Moderator { Name = "Moderator3", Email = "moderator3@test.com" }
            };

            var moderatorsDbSet = DbSetMockFactory.GetDbSetAsyncMock(moderators);

            // Act
            var result = await _handler.Handle(new GetModeratorsCommand("moderator1"), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            result.IfSucc(x =>
            {
                var size = x.Count;
                Assert.Equal(1, size);
                Assert.Equal("Moderator1", x[0].Name);
            });
        }
    }
}
