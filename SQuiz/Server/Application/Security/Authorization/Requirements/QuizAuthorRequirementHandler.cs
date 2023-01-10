using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using System.Security.Claims;

namespace SQuiz.Server.Application.Security.Authorization.Requirements
{
    public class QuizAuthorRequirementHandler : AuthorizationHandler<QuizAuthorRequirement>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISQuizContext _quizContext;

        public QuizAuthorRequirementHandler(IHttpContextAccessor httpContext,
            ISQuizContext quizContext)
        {
            _httpContext = httpContext;
            _quizContext = quizContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, QuizAuthorRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (_httpContext.HttpContext?.GetRouteValue("resourceId") is string resourceId &&
                await _quizContext.Quizzes.AnyAsync(x => x.Id == resourceId && (x.IsPublic || x.AuthorId == userId)))
            {
                context.Succeed(requirement);
            }
        }
    }
}
