using AutoMapper;
using LanguageExt;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Models;
using Unit = Mediator.Unit;

namespace SQuiz.Application.Games.SendAnswer
{
    public class SendAnswerCommand : IRequest<Result<ReceivedPointsDto>>
    {
        public SendAnswerCommand(string playerId, SendAnswerDto model)
        {
            PlayerId = playerId;
            Model = model;
        }

        public string PlayerId { get; set; }
        public SendAnswerDto Model { get; set; }
    }

    public class SendAnswerCommandHandler : IRequestHandler<SendAnswerCommand, Result<ReceivedPointsDto>>
    {
        private readonly ISQuizContext _context;
        private readonly IPointsCounter _pointsCounter;
        private readonly IMapper _mapper;

        public SendAnswerCommandHandler(ISQuizContext context, IPointsCounter pointsCounter, IMapper mapper)
        {
            _context = context;
            _pointsCounter = pointsCounter;
            _mapper = mapper;
        }

        public async ValueTask<Result<ReceivedPointsDto>> Handle(SendAnswerCommand request, CancellationToken cancellationToken)
        {

            string playerId = request.PlayerId;
            var answerDto = request.Model;

            var result = await (from question in GetQuestion(request)
                                from points in CountPoints(request, question)
                                from savedAnswer in SaveAnswer(request, points, question)
                                from player in GetPlayer(request)
                                select new { question, points, player })
                                .Match(
                x => GetReceivedPoints(request, x.question, x.points, x.player),
                e => new Result<ReceivedPointsDto>(new BadRequestException()));

            return result;
        }

        private TryAsync<Question> GetQuestion(SendAnswerCommand request) => async () =>
        {
            var question = await _context.Questions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Model.QuestionId);

            return question ?? new Result<Question>(new NotFoundException());
        };

        private TryAsync<int> CountPoints(SendAnswerCommand request, Question question) => async () =>
        {
            bool isCorrect = request.Model.AnswerId != null && question.CorrectAnswerId == request.Model.AnswerId;
            int points = 0;

            if (isCorrect)
            {
                points = _pointsCounter.GetPoints(request.Model.TimeToSolve, question.AnsweringTime, question.Points);
            }

            return points;
        };

        private TryAsync<Unit> SaveAnswer(SendAnswerCommand request, int points, Question question) => async () =>
        {
            var playerAnswer = new PlayerAnswer()
            {
                Id = Guid.NewGuid().ToString(),
                AnswerId = request.Model.AnswerId,
                PlayerId = request.PlayerId,
                Points = points,
                CorrectAnswerId = question.CorrectAnswerId,
                Order = question.Order,
            };

            if (!await _context.PlayerAnswers.AnyAsync(x => x.PlayerId == request.PlayerId && x.Order == question.Order))
            {
                _context.PlayerAnswers.Add(playerAnswer);
                await _context.SaveChangesAsync();
            }

            return Unit.Value;
        };

        private ReceivedPointsDto GetReceivedPoints(SendAnswerCommand request, Question question, int points, Player player)
        {
            var receivedPoints = new ReceivedPointsDto()
            {
                CorrectAnswerId = question.CorrectAnswerId,
                CurrentPoints = points,
                SelectedAnswerId = request.Model.AnswerId,
                TotalPoints = player.PlayerAnswers.Sum(x => x.Points),
                GameShortId = player.QuizGame?.ShortId ?? 0,
                Player = _mapper.Map<PlayerDto>(player) 
            };

            return receivedPoints;
        }

        private TryAsync<Player> GetPlayer(SendAnswerCommand request) => async () =>
        {
            return await _context.Players
                .Include(x => x.PlayerAnswers)
                .Include(x => x.QuizGame)
                .Include(x => x.RegularQuizGame)
                .Include(x => x.RegularQuizGame)
                .FirstOrDefaultAsync(x => x.Id == request.PlayerId)
                ?? new Result<Player>(new NotFoundException());
        };
    }
}
