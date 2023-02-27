using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Interfaces;
using SQuiz.Shared.Services;

namespace SQuiz.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddTransient<IValidator<EditQuizDto>, EditQuizDtoValidator>();
            services.AddTransient<IPointsCounter, PointsCounter>();
            services.AddScoped<PlayRealtimeGameService>();
            services.AddScoped<IPlayGameService>(x => x.GetRequiredService<PlayRealtimeGameService>());
            services.AddScoped<IPlayRealtimeGameService>(x => x.GetRequiredService<PlayRealtimeGameService>());           
            
            return services;
        }
    }
}
