using Application.Dtos.Game;
using Application.Dtos.Prevention;
using Application.Services.Identity;
using Domain.Entitites;
using Infrastructure.Repositories;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;
public interface IGameService
{
    Task<bool> AddGame(GameDto dto);
    Task<IEnumerable<GameDto>> GetGame();
}

public class GameService : IGameService
{
    private readonly IBaseRepository<Game> gameRep;
    private readonly IBaseRepository<Professor> professorRep;
    private readonly IGameRep gameRepo;
    private readonly IIdentityService userRep;

    public GameService(IBaseRepository<Game> gameRep, IBaseRepository<Professor> professorRep, IIdentityService userRep, IGameRep gameRepo)
    {
        this.gameRep = gameRep;
        this.professorRep = professorRep;
        this.userRep = userRep;
        this.gameRepo = gameRepo;
    }

    public async Task<bool> AddGame(GameDto dto)
    {
        var professor = await userRep.GetProfessorId();
        var today = DateTime.Today;
        var closestDeadline = dto.Round
            .OrderBy(x => Math.Abs((x.Deadline - today).Ticks))
            .FirstOrDefault()?.Deadline; 

        var students = await userRep.GetStudents(dto.StudentsId);

        var round = new Round()
        {
            Deadline = closestDeadline ?? today,
            CurrentRound = 1,
        };

        var game = new Game()
        {
            ProfessorEmail = professor,
            Rounds = new List<Round>() { round },
            Students = students,
        };

        await gameRep.AddAsync(game);

        return true;
    }

    public async Task<IEnumerable<GameDto>> GetGame()
    {
        var game = from gamee in await gameRepo.GetAllGame()
                   select new GameDto()
                   {
                       Id = gamee.Id,
                       ProfessorId = gamee.Id,
                       Round = gamee.Rounds.Select(x => new Dtos.Round.RoundDto()
                       {
                           Id = x.Id,
                           Deadline = x.Deadline,
                       }),
                       StudentsId = gamee.Students.Select(x => x.Id).ToList(),
                   };

        return game;
    }
}

