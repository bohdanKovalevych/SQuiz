using AutoMapper;
using LanguageExt.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Models;

namespace SQuiz.Application.Games.GetQuestion
{
    public class GetQuestionCommand : IRequest<Result<GameQuestionDto?>>
    {
        public GetQuestionCommand(int gameShortId, int lastQuestionIndex)
        {
            GameShortId = gameShortId;
            LastQuestionIndex = lastQuestionIndex;
        }

        public int GameShortId { get; set; }
        public int LastQuestionIndex { get; set; }
        public Action<int>? OnIndexChanged { get; set; }
        public Func<Task>? OnEndQuiz { get; set; }
        public Func<Task>? OnStartQuiz { get; set; }
    }

    public class GetQuestionCommandHandler : IRequestHandler<GetQuestionCommand, Result<GameQuestionDto?>>
    {
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;

        public GetQuestionCommandHandler(ISQuizContext quizContext, IMapper mapper)
        {
            _context = quizContext;
            _mapper = mapper;
        }

        public async ValueTask<Result<GameQuestionDto?>> Handle(GetQuestionCommand request, CancellationToken cancellationToken)
        {
            int gameShortId = request.GameShortId;
            int index = request.LastQuestionIndex;

            if (index == 0 && request.OnStartQuiz != null)
            {
                await request.OnStartQuiz();
            }

            var question = await _context.Questions
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x =>
                    x.Quiz.QuizGames.Any(x => x.ShortId == gameShortId)
                    && x.Order == index, cancellationToken);

            if (await TryEndQuiz(question, request))
            {
                return null;
            }

            request.OnIndexChanged?.Invoke(index + 1);
            question.Answers = question.Answers.OrderBy(x => x.Order).ToList();

            var questionDto = _mapper.Map<GameQuestionDto>(question);
            return questionDto;
        }

        private async Task<bool> TryEndQuiz(Question? question, GetQuestionCommand request)
        {
            bool isEndQUiz = false;

            if (question == null)
            {
                isEndQUiz = true;
            }

            if (question == null && request.OnEndQuiz != null)
            {
                await request.OnEndQuiz();
            }

            return isEndQUiz;
        }
    }
}
