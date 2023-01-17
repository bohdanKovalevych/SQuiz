using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared;
using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ManageGamesController : ControllerBase
    {
        private readonly ISQuizContext _context;

        public ManageGamesController(ISQuizContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGameOptions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _context.QuizGames
                .Where(x => x.StartedById == userId)
                .Select(x => new GameOptionDto()
                {
                    QuizId = x.QuizId,
                    EndDate = x.DateEnd,
                    Id = x.Id,
                    QuestionCount = x.Quiz.Questions.Count,
                    ShortId = x.ShortId,
                    Name = x.Quiz.Name,
                    StartDate = x.DateStart
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPut("{resourceId}")]
        [Authorize(Policies.QuizModerator)]
        public async Task<IActionResult> EditQuizGame(
            [FromRoute] string resourceId,
            [FromBody] StartGameDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var game = new QuizGame()
            {
                Id = model.Id,
                DateEnd = model.EndDate,
                ShortId = model.ShortId,
                DateStart = model.StartDate,
                QuizId = resourceId
            };

            if (model.Id != null && !await _context.QuizGames.AnyAsync(x => x.Id == model.Id))
            {
                return NotFound();
            }

            game.DateUpdated = DateTime.Now;
            game.StartedById = userId;

            if (!await _context.Moderators.AnyAsync(x => x.Id == userId))
            {
                _context.Moderators.Add(new Moderator()
                {
                    Id = userId,
                    Email = User.FindFirstValue("preferred_username"),
                    Name = User.FindFirstValue("name"),
                });

                await _context.SaveChangesAsync();
            }

            if (game.Id == null)
            {
                game.Id = Guid.NewGuid().ToString();
                _context.QuizGames.Add(game);
            }
            else
            {
                _context.QuizGames.Update(game);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
