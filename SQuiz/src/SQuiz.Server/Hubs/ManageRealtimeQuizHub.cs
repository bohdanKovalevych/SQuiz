using AutoMapper;
using LanguageExt;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Games.GetQuestion;
using SQuiz.Application.Games.SetEmptyAnswersForPlayers;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Hubs.ManageRealtimeQuizHub;
using SQuiz.Shared.Hubs.RealtimeQuizHub;
using SQuiz.Shared.Models;
using System.Security.Claims;

namespace SQuiz.Server.Hubs
{
    [Authorize]
    public class ManageRealtimeQuizHub : Hub<IManageRealtimeQuizHubPush>, IManageRealtimeQuizHubInvoke
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ISQuizContext _quizContext;
        private readonly IHubContext<RealtimeQuizHub, IRealtimeQuizHubPush> _quizHub;
        private readonly ILogger<ManageRealtimeQuizHub> _logger;

        public ManageRealtimeQuizHub(
            IMapper mapper,
            IMediator mediator,
            ISQuizContext quizContext,
            IHubContext<RealtimeQuizHub, IRealtimeQuizHubPush> quizHub,
            ILogger<ManageRealtimeQuizHub> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _quizContext = quizContext;
            _quizHub = quizHub;
            _logger = logger;
        }

        public async Task NextQuestion(int shortId)
        {
            var playerId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var game = await _quizContext.RealtimeQuizGames
                .Include(x => x.Players)
                    .ThenInclude(x => x.PlayerAnswers)
                .FirstAsync(x => x.ShortId == shortId)
                .Bind(async (x) =>
                {
                    var command = new GetQuestionCommand(x.ShortId, x.CurrentQuestionIndex)
                    {
                        OnEndQuiz = async () =>
                        {
                            var scoresDto = _mapper.Map<List<PlayerDto>>(x.Players);
                            await _quizHub.Clients.Group(shortId.ToString()).OnEndQuiz(scoresDto);
                        },
                        OnIndexChanged = (index) =>
                        {
                            x.CurrentQuestionIndex = index;
                            _quizContext.SaveChangesAsync().Wait();
                        },
                        OnStartQuiz = async () =>
                        {
                            ResetPoints(x);
                            await _quizContext.SaveChangesAsync();
                            await _quizHub.Clients.Group(shortId.ToString()).OnStartQuiz();
                        }
                    };
                    var result = await _mediator.Send(command);

                    return result;
                    ;
                });

            await game.Match(
                x => _quizHub.Clients.Group(shortId.ToString()).OnGetQuestion(x),
                ErrorHandler);
        }

        private void ResetPoints(RealtimeQuizGame game)
        {
            foreach (var player in game.Players)
            {
                player.Points = 0;
                _quizContext.PlayerAnswers.RemoveRange(player.PlayerAnswers);
            }
        }

        private Task ErrorHandler(Exception e)
        {
            _logger.LogError(e, e.Message);
            return Clients.Caller.OnError(e.Message);
        }

        public async Task TimeEnd(int shortId)
        {
            var setEmptyCommand = new SetEmptyAnswersForPlayersCommand()
            {
                GameShortId = shortId,
                ProcessAnswer = async command => (await _mediator.Send(command)).Match(x => x, e => 
                {
                    _logger.LogError(e, "on proccess answer");
                    return null;
                })
            };

            var result = await _mediator.Send(setEmptyCommand);

            await result.Match(async x =>
            {
                var (points, correctAnswer) = x;
                await _quizHub.Clients.Group(shortId.ToString())
                    .OnAllPlayersAnswered(points, correctAnswer);
            }, e => 
            {
                _logger.LogError(e, "on proccess answer");
                return Task.CompletedTask;
            });
        }
    }
}
