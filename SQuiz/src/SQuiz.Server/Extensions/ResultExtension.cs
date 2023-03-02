using LanguageExt.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using SQuiz.Shared.Exceptions;

namespace SQuiz.Server.Extensions
{
    public static class ResultExtension
    {
        public static IActionResult MatchAction<T>(this Result<T> result)
        {
            return result.Match<IActionResult>(
                x => 
                {
                    if (x == null || x is Unit)
                    {
                        return new NoContentResult();
                    }
                    return new OkObjectResult(x);
                },
                ex =>
                {
                    if (ex is NotFoundException)
                    {
                        return new NotFoundResult();
                    }
                    else if (ex is BadRequestException e && e.Message is string validationMessage)
                    {
                        return new BadRequestObjectResult(validationMessage);
                    }

                    return new BadRequestResult();
                });
        }
    }
}
