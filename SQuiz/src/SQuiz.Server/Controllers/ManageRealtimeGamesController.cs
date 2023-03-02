using AutoMapper;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Application.ManageGames.EditQuizGame;
using SQuiz.Server.Extensions;
using SQuiz.Server.Hubs;
using SQuiz.Shared;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Hubs.RealtimeQuizHub;
using SQuiz.Shared.Models;
using System.Security.Claims;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ManageRealtimeGamesController : ControllerBase
    {
        private readonly ISQuizContext _quizContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHubContext<RealtimeQuizHub, IRealtimeQuizHubPush> _hubContext;

        public ManageRealtimeGamesController(
            ISQuizContext quizContext, 
            IMediator mediator,
            IMapper mapper,
            IHubContext<RealtimeQuizHub, IRealtimeQuizHubPush> hubContext)
        {
            _quizContext = quizContext;
            _mediator = mediator;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet("{shortId}")]
        public async Task<IActionResult> GetGame(int shortId)
        {
            var game = await _quizContext.QuizGames
                .Include(x => ((RealtimeQuizGame)x).Quiz)
                    .ThenInclude(x => x.Questions)
                .FirstOrDefaultAsync(x => x.ShortId == shortId);
            
            var result = _mapper.Map<RealtimeGameOptionDto>(game);
            
            return Ok(result);
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizModerator)]
        public async Task<IActionResult> EditRealtimeQuizGame(
            [FromRoute] string resourceId, 
            [FromBody] StartRealtimeGameDto startGame) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue("preferred_username");
            var name = User.FindFirstValue("name");
            startGame.QuizId = resourceId;
            var command = new EditRealtimeQuizGameCommand()
            {
                Model = startGame,
                UserId = userId,
                CreateModeratorIfNotExist = async () =>
                {
                    _quizContext.Moderators.Add(new Moderator()
                    {
                        Id = userId,
                        Email = User.FindFirstValue("preferred_username"),
                        Name = User.FindFirstValue("name"),
                    });
                    await _quizContext.SaveChangesAsync();
                }
            };
            var result = await _mediator.Send(command);
            
            return result.MatchAction();
        }
    }
}
