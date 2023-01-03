using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SQuiz.Shared.Dtos.Quiz;

namespace SQuiz.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddTransient<IValidator<EditQuizDto>, EditQuizDtoValidator>();

            return services;
        }
    }
}
