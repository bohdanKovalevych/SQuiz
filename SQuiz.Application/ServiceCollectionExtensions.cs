using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SQuiz.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}