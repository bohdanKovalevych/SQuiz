using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQuiz.Client;
using SQuiz.Infrastructure.Interfaces;

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
                .Where(x => DateTime.Now >= x.QuizGame.DateStart)
                .Where(x => DateTime.Now <= x.QuizGame.DateEnd)
                .AnyAsync())
            {
                context.Succeed(requirement);
            }
        }
    }
}
