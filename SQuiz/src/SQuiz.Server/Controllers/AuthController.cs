using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Identity.Models;
using SQuiz.Shared.Models;
using System.Security.Claims;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<SQuizUser> _userManager;
        private readonly ISQuizContext _quizContext;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<SQuizUser> userManager, ISQuizContext quizContext, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _quizContext = quizContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SyncUser([FromBody] string action)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                user = new SQuizUser()
                {
                    Email = User.FindFirstValue("preferred_username"),
                    UserName = User.FindFirstValue("name"),
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    EmailConfirmed = true
                };

                var userResult = await _userManager.CreateAsync(user);

                if (!userResult.Succeeded)
                {
                    foreach (var err in userResult.Errors)
                    {
                        _logger.LogError(err.Code, err.Description);
                    }

                    return Ok();
                }
            }

            if (!await _quizContext.Moderators.AnyAsync(x => x.Id == id))
            {
                var moderator = new Moderator()
                {
                    Id = id,
                    Email = user.Email,
                    Name = user.UserName
                };
                _quizContext.Moderators.Add(moderator);
                var numSaved = await _quizContext.SaveChangesAsync();

                if (numSaved == 0)
                {
                    _logger.LogError("Moderator was not saved");
                }
            }

            return Ok();
        }
    }
}
