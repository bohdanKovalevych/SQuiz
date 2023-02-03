using LanguageExt.Common;
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
                    if (x == null)
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
                    else if (ex is BadRequestException)
                    {
                        return new BadRequestResult();
                    }

                    return new BadRequestResult();
                });
        }
    }
}
