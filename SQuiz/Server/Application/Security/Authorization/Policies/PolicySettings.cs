using Microsoft.AspNetCore.Authorization;
using SQuiz.Server.Application.Security.Authorization.Requirements;
using Core = SQuiz.Shared;

namespace SQuiz.Server.Application.Security.Authorization.Policies
{
    public static class PolicySettings
    {
        public static void SetPolicySettings(AuthorizationOptions authOptions)
        {
            authOptions.AddPolicy(Core.Policies.PlayerInGame, builder =>
            {
                builder.AddRequirements(new PlayerInGameRequirement());
            });

            authOptions.AddPolicy(Core.Policies.QuizAuthor, builder =>
            {
                builder.RequireAuthenticatedUser()
                .AddRequirements(new QuizAuthorRequirement());
            });

            authOptions.AddPolicy(Core.Policies.QuizModerator, builder =>
            {
                builder.RequireAuthenticatedUser()
                .AddRequirements(new QuizModeratorRequirement());
            });
        }
    }
}
