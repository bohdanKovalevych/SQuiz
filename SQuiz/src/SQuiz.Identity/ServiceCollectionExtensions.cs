using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SQuiz.Identity.Data;
using SQuiz.Identity.Models;

namespace SQuiz.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityByConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQuizDb");

            services.AddDbContext<IdentityContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentityCore<SQuizUser>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddSignInManager<SignInManager<SQuizUser>>()
                .AddUserManager<UserManager<SQuizUser>>();

            return services;
        }

    }
}