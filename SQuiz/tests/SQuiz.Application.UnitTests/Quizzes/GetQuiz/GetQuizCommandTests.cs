using NSubstitute;
using SQuiz.Application.Quizzes.GetQuiz;
using SQuiz.Application.Services;
using SQuiz.Application.UnitTests.Helpers;
using SQuiz.Shared.Models;

namespace SQuiz.Application.UnitTests.Quizzes.GetQuiz
{
    public class GetQuizCommandTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _service;
        private readonly QuizService _quizService;

        public GetQuizCommandTests(ServiceFixture service)
        {
            _service = service;
            _quizService = new QuizService();
        }

        [Fact]
        public async Task GetQuiz_WhenNotFound_ReturnsFailedResult()
        {
            // Arrange
            var command = new GetQuizCommand("not existing id");
            var quizzesDbSet = DbSetMockFactory.GetDbSetAsyncMock(new List<Quiz>());
            _service.QuizContext.Quizzes.Returns(quizzesDbSet);

            var handler = new GetQuizCommandHandler(_service.QuizContext, _service.QuizService, _service.GetMapper());

            // Act
            var result = await handler.Handle(command, default);

            //Assert

            Assert.True(result.IsFaulted);
        }

        [Fact]
        public async Task GetQuiz_WhenFound_ReturnsSuccededResultWithOrderedAnswersAndWithRightAnswerIndex()
        {
            // Arrange
            var command = new GetQuizCommand("1");
            var quizzesDbSet = DbSetMockFactory.GetDbSetAsyncMock(new List<Quiz>()
            {
                new Quiz()
                {
                    Id = "1",
                    Name = "name",
                    Questions = new List<Question>
                    {
                        new Question()
                        {
                            QuestionText = "question 1",
                            Order = 0,
                            Id = "q1",
                            CorrectAnswerId = "a2",
                            Answers = new List<Answer>()
                            {
                                new Answer()
                                {
                                    Id = "a2",
                                    AnswerText = "answer 2 is correct",
                                    Order = 1,
                                    QuestionId = "q1"
                                },
                                new Answer()
                                {
                                    Id = "a1",
                                    AnswerText = "answer 1 is incorrect",
                                    Order = 0,
                                    QuestionId = "q1"
                                }
                            }
                        }
                    }
                }
            });
            _service.QuizContext.Quizzes.Returns(quizzesDbSet);
            
            var handler = new GetQuizCommandHandler(_service.QuizContext, _quizService, _service.GetMapper());

            // Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            var quiz = result.Match(x => x, _ => default);
            Assert.Equal(1, quiz.Questions[0].CorrectAnswerIndex);
            Assert.Equal(0, quiz.Questions[0].Answers[0].Order);
            Assert.Equal(1, quiz.Questions[0].Answers[1].Order);
        }

        [Fact]
        public async Task GetQuiz_WhenFound_ReturnsSuccededResultWithOrderedQuestions()
        {
            // Arrange
            var command = new GetQuizCommand("1");
            var quizzesDbSet = DbSetMockFactory.GetDbSetAsyncMock(new List<Quiz>()
            {
                new Quiz()
                {
                    Id = "1",
                    Name = "name",
                    Questions = new List<Question>
                    {
                        new Question()
                        {
                            QuestionText = "question 2",
                            Order = 1,
                            Id = "q2",
                        },
                        new Question()
                        {
                            QuestionText = "question 1",
                            Order = 0,
                            Id = "q1",
                        }
                    }
                }
            });
            _service.QuizContext.Quizzes.Returns(quizzesDbSet);

            var handler = new GetQuizCommandHandler(_service.QuizContext, _quizService, _service.GetMapper());

            // Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.True(result.IsSuccess);
            var quiz = result.Match(x => x, _ => default);
            Assert.Equal(0, quiz.Questions[0].Order);
            Assert.Equal(1, quiz.Questions[1].Order);
        }
    }
}
