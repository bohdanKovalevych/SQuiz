using AutoMapper;
using NSubstitute;
using SQuiz.Application.Quizzes.UpdateQuiz;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;
using SQuiz.Shared.Models.Interfaces;

namespace SQuiz.Application.UnitTests.Quizzes.UpdateQuiz
{
    public class UpdateQuizCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly UpdateQuizCommandHandler _handler;
        private readonly IMapper _mapper;

        public UpdateQuizCommandTests(ServiceFixture service)
        {
            _service = service;
            _mapper = service.GetMapper();
            _handler = new UpdateQuizCommandHandler(_service.QuizContext, _mapper, _service.QuizService);
        }

        [Fact]
        public async Task Handle_NotFoundQuiz_ReturnsNotFoundException()
        {
            // Arrange
            var request = new UpdateQuizCommand(new EditQuizDto() { Id = "100" });
            var quizDbSet = DbSetMockFactory.GetDbSetAsyncMock(new List<Quiz>());
            _service.QuizContext.Quizzes
                .Returns(quizDbSet);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFaulted);
            result.IfFail(e => Assert.IsType<NotFoundException>(e));
        }

        [Fact]
        public async Task Handle_RemovesQuestionsAndAnswers()
        {
            // Arrange
            var request = new UpdateQuizCommand(new EditQuizDto()
            {
                Id = "1",
                Questions = new List<QuestionDto>()
                    {
                        new QuestionDto()
                        {
                            Id = "q1",
                            Answers = new List<AnswerDto>()
                            {
                                new AnswerDto()
                                {
                                    Id = "a1"
                                }
                            }
                        }
                    }
            });

            Quiz quiz = new Quiz()
            {
                Id = "1",
                Questions = new List<Question>()
                {
                    new Question()
                    {
                        Id = "q1",
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                Id = "a1"
                            }
                        }
                    }
                }
            };
            var quizDbSet = DbSetMockFactory.GetDbSetAsyncMock(new[] { quiz });
            _service.QuizContext.Quizzes
                .Returns(quizDbSet);

            var questions = new List<Question>(quiz.Questions);
            _service.QuizService.GetQuestionsToRemove(Arg.Any<Quiz>(), Arg.Any<EditQuizDto>())
                .Returns(questions);

            var answers = new List<Answer>(quiz.Questions.SelectMany(x => x.Answers));
            _service.QuizService.GetAnswersToRemove(Arg.Any<Quiz>(), Arg.Any<EditQuizDto>())
                .Returns(answers);

            //Act
            await _handler.Handle(request, CancellationToken.None);

            //Assert
            await _service.QuizContext.Received(4).SaveChangesAsync(Arg.Any<CancellationToken>());
            _service.QuizContext.Received(2).Questions.RemoveRange(questions);
            _service.QuizContext.Received(1).Answers.RemoveRange(answers);
            _service.QuizService.Received(1).GetQuestionsToRemove(quiz, request.Model);
            _service.QuizService.Received(1).GetAnswersToRemove(quiz, request.Model);
            _service.QuizService.Received(1).AssignIdsToQuestionsAndAnswers(quiz,
                Arg.Any<Action<IEntity>>(),
                Arg.Any<Action<IEntity>>());
            _service.QuizService.Received(1).SetCorrectAnswersFromModel(quiz, request.Model.Questions);
            _service.QuizContext.Received(2).Quizzes.Update(quiz);
        }
    }
}
