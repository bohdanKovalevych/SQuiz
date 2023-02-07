using AutoMapper;
using AutoMapper.QueryableExtensions;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Quiz;
using SQuiz.Shared.Extensions;

namespace SQuiz.Application.Quizzes.GetModerators
{
    public class GetModeratorsCommand : IRequest<Result<List<ModeratorDto>>>
    {
        public GetModeratorsCommand(string? searchQuery)
        {
            SearchQUery = searchQuery;
        }

        public string? SearchQUery { get; set; }
    }

    public class GetModeratorsCommandHandler : IRequestHandler<GetModeratorsCommand, Result<List<ModeratorDto>>>
    {
        private readonly ISQuizContext _quizContext;
        private readonly IMapper _mapper;

        public GetModeratorsCommandHandler(ISQuizContext quizContext, IMapper mapper)
        {
            _quizContext = quizContext;
            _mapper = mapper;
        }

        public async ValueTask<Result<List<ModeratorDto>>> Handle(GetModeratorsCommand request, CancellationToken cancellationToken)
        {
            string q = request.SearchQUery ?? string.Empty;
            var moderators = await _quizContext.Moderators
                .WithNameOrEmail(q)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ModeratorDto>>(moderators);
        }
    }
}
