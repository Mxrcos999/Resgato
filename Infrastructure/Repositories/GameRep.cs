using Domain.Entitites;
using Infrastructure.Context;
using Infrastructure.Repositories.BaseRepository;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface IGameRep 
{
    Task<IEnumerable<Game>> GetAllGame();
}
public class GameRep : IGameRep
{
    private readonly ApplicationContext _context;

    public GameRep(ApplicationContext context)
    {
        _context = context;

    }

    public async Task<IEnumerable<Game>> GetAllGame()
    {
        var games = from game in _context.Game
                    .Include(x => x.Rounds)
                    .Include(x => x.Students)
                    .Include(x => x.Rounds)
                    select new Game
                    {
                        Id = game.Id,
                        Students = game.Students,
                        Rounds = game.Rounds,
                        ProfessorEmail = game.ProfessorEmail,

                    };

        return games.AsEnumerable();
    }
}
