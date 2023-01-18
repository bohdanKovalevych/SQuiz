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

        public AuthController(UserManager<SQuizUser> userManager, ISQuizContext quizContext)
        {
            _userManager = userManager;
            _quizContext = quizContext;
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
                    // TODO: log error
                    return Ok();
                }
            }

            if (!await _quizContext.Moderators.AnyAsync(x => x.Id == id))
            {
                var moderator = new Moderator()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    Name = user.UserName
                };
                _quizContext.Moderators.Add(moderator);
                var numSaved = await _quizContext.SaveChangesAsync();

                if (numSaved == 0)
                {
                    // TODO: log error
                }
            }

            return Ok();
        }
    }
}
