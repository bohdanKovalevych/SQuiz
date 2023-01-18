using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using System.Security.Claims;

namespace SQuiz.Server.Application.Security.Authorization.Requirements
{
    public class QuizModeratorRequirementHandler : AuthorizationHandler<QuizModeratorRequirement>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISQuizContext _quizContext;

        public QuizModeratorRequirementHandler(IHttpContextAccessor httpContext, ISQuizContext quizContext)
        {
            _httpContext = httpContext;
            _quizContext = quizContext;

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, QuizModeratorRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (_httpContext.HttpContext?.GetRouteValue("resourceId") is string resourceId &&
                await _quizContext.Quizzes.AnyAsync(x => x.Id == resourceId 
                && (x.IsPublic || x.AuthorId == userId 
                || x.QuizModerators.Any(y => y.ModeratorId == userId))))
            {
                context.Succeed(requirement);
            }
        }
    }
}
