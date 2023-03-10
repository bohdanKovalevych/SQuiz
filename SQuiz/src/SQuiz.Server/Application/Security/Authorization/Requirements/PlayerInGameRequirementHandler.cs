using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Client;

namespace SQuiz.Server.Application.Security.Authorization.Requirements
{
    public class PlayerInGameRequirementHandler : AuthorizationHandler<PlayerInGameRequirement>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISQuizContext _quizContext;

        public PlayerInGameRequirementHandler(IHttpContextAccessor httpContext, ISQuizContext quizContext)
        {
            _httpContext = httpContext;
            _quizContext = quizContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PlayerInGameRequirement requirement)
        {
            if (_httpContext.HttpContext?.Request.Cookies[Constants.CookiesKey.PlayerId] is string playerId
                && await _quizContext.Players
                .Where(x => x.Id == playerId)
                .Where(x => x.RegularQuizGame != null && DateTime.Now >= x.RegularQuizGame.DateStart)
                .Where(x => x.RegularQuizGame != null && DateTime.Now <= x.RegularQuizGame.DateEnd)
                .AnyAsync())
            {
                context.Succeed(requirement);
            }
        }
    }
}
