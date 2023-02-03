using FluentValidation;

namespace SQuiz.Shared.Dtos.Quiz
{
    public class EditQuizDtoValidator : AbstractValidator<EditQuizDto>
    {
        public EditQuizDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(2)
                .MaximumLength(300)
                .NotEmpty();

            RuleForEach(x => x.Questions)
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.QuestionText)
                        .MinimumLength(1)
                        .MaximumLength(300)
                        .NotEmpty();
                    
                    x.RuleFor(x => x.Answers.Count)
                        .LessThanOrEqualTo(6)
                        .GreaterThanOrEqualTo(2);

                    x.RuleForEach(x => x.Answers)
                        .ChildRules(x =>
                        {
                            x.RuleFor(x => x.AnswerText)
                                .MinimumLength(1)
                                .MaximumLength(100)
                                .NotEmpty();
                        });
                });
        }
    }
}
