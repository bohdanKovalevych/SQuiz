using AutoMapper;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Games.SendAnswer;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;

namespace SQuiz.Application.Games.SetEmptyAnswersForPlayers
{
    public class SetEmptyAnswersForPlayersCommand : IRequest<Result<(List<ReceivedPointsDto>, string)>>
    {
        public int GameShortId { get; set; }
        public Func<SendAnswerCommand, Task<ReceivedPointsDto>>? ProcessAnswer { get; set; }
    }

    public class SetEmptyAnswersForPlayersCommandHandler : IRequestHandler<SetEmptyAnswersForPlayersCommand, Result<(List<ReceivedPointsDto>, string)>>
    {
        private readonly ISQuizContext _quizContext;
        private readonly IMapper _mapper;

        public SetEmptyAnswersForPlayersCommandHandler(ISQuizContext quizContext, IMapper mapper)
        {
            _quizContext = quizContext;
            _mapper = mapper;
        }

        public async ValueTask<Result<(List<ReceivedPointsDto>, string)>> Handle(SetEmptyAnswersForPlayersCommand request, CancellationToken cancellationToken)
        {
            var game = await _quizContext.RealtimeQuizGames.AsNoTracking()
                .Include(x => x.Players)
                    .ThenInclude(x => x.PlayerAnswers)
                .Where(x => x.ShortId == request.GameShortId)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return new Result<(List<ReceivedPointsDto>, string)>(new NotFoundException());
            }

            var currentOrder = game.CurrentQuestionIndex;
            var question = await _quizContext.Questions
                .Where(x => x.Quiz.QuizGames.Any(x => x.ShortId == request.GameShortId) && x.Order + 1 == currentOrder)
                .FirstOrDefaultAsync();

            if (question == null || question.CorrectAnswerId == null)
            {
                return new Result<(List<ReceivedPointsDto>, string)>(new NotFoundException());
            }

            var result = new List<ReceivedPointsDto>();
     
            foreach (var player in game.Players)
            {
                var points = player.PlayerAnswers
                    .Where(x => x.Order == question.Order)
                    .Select(x => new ReceivedPointsDto()
                    {
                        CorrectAnswerId = x.CorrectAnswerId,
                        CurrentPoints = x.Points,
                        GameShortId = request.GameShortId,
                        Player = _mapper.Map<PlayerDto>(player),
                        SelectedAnswerId = x.AnswerId,
                        TotalPoints = player.Points
                    })
                    .FirstOrDefault();

                if (points == null && request.ProcessAnswer != null)
                {
                    var command = new SendAnswerCommand(player.Id, new SendAnswerDto(question.Id));
                    points = await request.ProcessAnswer(command);
                }

                result.Add(points);
            }

            return (result, question.CorrectAnswerId);
        }
    }
}
