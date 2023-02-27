using AutoMapper;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using SQuiz.Application.Interfaces;
using SQuiz.Shared.Dtos.Game;
using SQuiz.Shared.Exceptions;
using SQuiz.Shared.Models;

namespace SQuiz.Application.ManageGames.EditQuizGame
{
    public abstract class EditQuizGameCommand<TGame, TStartGameDto>
        where TGame : QuizGame
        where TStartGameDto : StartGameDto
    {
        public string UserId { get; set; }
        public Func<Task> CreateModeratorIfNotExist { get; set; }
        public TStartGameDto Model { get; set; }
    }

    public abstract class EditQuizGameCommandHandler<TGame, TStartGameDto>
        where TGame : QuizGame
        where TStartGameDto : StartGameDto
    {
        private readonly ISQuizContext _context;
        private readonly IMapper _mapper;

        public EditQuizGameCommandHandler(ISQuizContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async ValueTask<Result<int>> Handle(EditQuizGameCommand<TGame, TStartGameDto> request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var game = _mapper.Map<TGame>(request.Model);
            var model = request.Model;

            if (model.Id != null && !await _context.Set<TGame>().AnyAsync(x => x.Id == model.Id, cancellationToken))
            {
                return new Result<int>(new NotFoundException());
            }

            game.DateUpdated = DateTime.Now;
            game.StartedById = userId;

            if (!await _context.Moderators.AnyAsync(x => x.Id == userId, cancellationToken))
            {
                await request.CreateModeratorIfNotExist();
            }

            if (game.Id == null)
            {
                game.Id = Guid.NewGuid().ToString();
                _context.Set<TGame>().Add(game);
            }
            else
            {
                _context.Set<TGame>().Update(game);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return game.ShortId;
        }
    }
}
