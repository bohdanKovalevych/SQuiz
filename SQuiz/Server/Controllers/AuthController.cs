using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQuiz.Identity.Models;

namespace SQuiz.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<SQuizUser> _userManager;

        public AuthController(UserManager<SQuizUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SyncUser()
        {

            return Ok();
        }
    }
}
