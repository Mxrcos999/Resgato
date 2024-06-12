using Domain.Entitites;
using Infrastructure.Context;
using System.Data.Entity;
using System.Security.Claims;

namespace Infrastructure.Repositories;

public interface IGameRep
{
    Task<IEnumerable<Game>> GetAllGame();
    Task<IEnumerable<Game>> GetAllByIdGame();
}
public class GameRep : IGameRep
{
    private readonly ApplicationContext _context;
    private readonly string _userId;


    public GameRep(ApplicationContext context)
    {
        _context = context;
        _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


    }

    public async Task<IEnumerable<Game>> GetAllGame()
    {
        var games = from game in _context.Game
                    .Include(x => x.Rounds)
                    .Include(x => x.Students)
                    .Include(x => x.Rounds)
                    .Include(x => x.Settings)
                    select new Game
                    {
                        Id = game.Id,
                        Concluded = game.Concluded,
                        GameName = game.GameName,
                        Students = game.Students,
                        Rounds = game.Rounds,
                        SettingId = game.SettingId,
                        Settings = game.Settings,
                        ProfessorEmail = game.ProfessorEmail,

                    };


        return games.AsEnumerable();
    }

    public async Task<IEnumerable<Game>> GetAllByIdGame()
    {
        var games = from game in _context.Game
                    .Include(x => x.Rounds)
                    .Include(x => x.Students)
                    .Include(x => x.Rounds)
                    .Include(x => x.Settings)
                    select new Game
                    {
                        Id = game.Id,
                        Concluded = game.Concluded,
                        GameName = game.GameName,
                        Students = game.Students,
                        Rounds = game.Rounds,
                        SettingId = game.SettingId,
                        Settings = game.Settings,
                        ProfessorEmail = game.ProfessorEmail,

                    };
        var gamesWhere = games.Where(game => game.Students.Any(student => student.Id == _userId)).ToList();

        return gamesWhere;
    }
}

public interface IAnswerRep
{
    Task<IEnumerable<Answers>> GetAllAnswer();
}
public class AnswerRep : IAnswerRep
{
    private readonly ApplicationContext _context;

    public AnswerRep(ApplicationContext context)
    {
        _context = context;

    }

    public async Task<IEnumerable<Answers>> GetAllAnswer()
    {
        var games = from answer in _context.Answers
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Game)
                    select new Answers
                    {
                        Id = answer.Id,
                        ApplicationUser = answer.ApplicationUser,
                        ApplicationUserId = answer.ApplicationUserId,
                        DateCastration = answer.DateCastration,
                        DeadLine = answer.DeadLine,
                        Game = answer.Game,
                        GameId = answer.GameId,
                        QuantityFemaleCastrate = answer.QuantityFemaleCastrate,
                        QuantityFemaleShelter = answer.QuantityFemaleShelter,
                        QuantityMaleCastrate = answer.QuantityMaleCastrate,
                        QuantityMaleShelter = answer.QuantityMaleShelter,
                        Round = answer.Round,
                        TotalPopulation = answer.TotalPopulation,
                        TotalPopulationCastrated = answer.TotalPopulationCastrated,
                        TotalPopulationFemaleCastrated = answer.TotalPopulationFemaleCastrated,
                        TotalPopulationMaleCastrated = answer.TotalPopulationMaleCastrated
                    };

        return games.AsEnumerable();
    }
}
