using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SQuiz.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediator(x => x.ServiceLifetime = ServiceLifetime.Scoped);

            return services;
        }
    }
}