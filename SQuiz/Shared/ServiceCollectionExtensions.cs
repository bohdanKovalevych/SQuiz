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

            return services;
        }
    }
}
