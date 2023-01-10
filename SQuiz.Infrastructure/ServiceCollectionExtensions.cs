using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQuiz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SQuiz.Infrastructure.Interfaces;
using SQuiz.Infrastructure.Services;

namespace SQuiz.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQuizDb");
            services.AddDbContext<SQuizContext>((serviceProvider, options) =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(connectionString);
            });
            
            services.AddScoped<ISQuizContext>(x => x.GetService<SQuizContext>());
            services.AddTransient<IModelService, ModelService>();
            
            return services;
        }
    }
}
