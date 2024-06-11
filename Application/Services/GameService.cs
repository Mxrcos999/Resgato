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
    Task<IEnumerable<GetGameDto>> GetGame();
    Task<GameInformation> GetInformationGame(int id);
}

public class GameService : IGameService
{
    private readonly IBaseRepository<Game> gameRep;
    private readonly IBaseRepository<Professor> professorRep;
    private readonly ISettingRep settingRep;
    private readonly IGameRep gameRepo;
    private readonly IIdentityService userRep;

    public GameService(IBaseRepository<Game> gameRep, IBaseRepository<Professor> professorRep, IIdentityService userRep, IGameRep gameRepo, ISettingRep settingRep)
    {
        this.gameRep = gameRep;
        this.professorRep = professorRep;
        this.userRep = userRep;
        this.gameRepo = gameRepo;
        this.settingRep = settingRep;
    }

    public async Task<bool> AddGame(GameDto dto)
    {
        var professor = await userRep.GetProfessorId();
        var today = DateTime.Today;

        var sortedRounds = dto.Round
            .OrderBy(x => Math.Abs((x.Deadline - today).Ticks))
            .ToList();
        var rounds = new List<Round>();

        for (int i = 0; i < sortedRounds.Count; i++)
        {
            var active = false;
            if(i + 1 == 1)
                active = true;

            var round = new Round()
            {
                Deadline = sortedRounds[i].Deadline,
                CurrentRound = i + 1,
                Active = active
            };

            rounds.Add(round);
        }
     
        var students = await userRep.GetStudents(dto.StudentsId);

        var game = new Game()
        {
            ProfessorEmail = professor,
            Rounds = rounds,
            Students = students,
        };

        await gameRep.AddAsync(game);

        return true;
    }

    public async Task<IEnumerable<GetGameDto>> GetGame()
    {
        var game = from gamee in await gameRepo.GetAllGame()
                   select new GetGameDto()
                   {
                       Id = gamee.Id,
                       TotalStudent = gamee.Students.Count,
                       ProfessorId = gamee.Id,

                       Round = gamee.Rounds.Select(x => new Dtos.Round.GetRoundDto()
                       {
                           Id = x.Id,
                           Deadline = x.Deadline,
                           CurrentRound = x.CurrentRound,
                           Active = x.Active
                           
                       }),
                       Students = gamee.Students.Select(x => new StudentDto()
                       {
                           Email = x.Email,
                           Id = x.Id,
                           Name = x.Name,   
                           StudentCode = x.StudentCode
                       }).ToList(),
                   };

        return game;
    }

    public async Task<GameInformation> GetInformationGame(int id)
    {
        var budget = await userRep.GetBudgetAsync();

        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == id).FirstOrDefault();
        if (game is null)
            return null;

        var settingList = (await settingRep.GetAllByIdAsync()).ToList();

        var gameDto = new GameInformation()
        {
            BudgetUser = budget,
            Id = id,
            TotalCatsFemale = settingList.Where(x => x.Gender == "Femea").FirstOrDefault().CatsQuantity,
            TotalCatsMale = settingList.Where(x => x.Gender == "MACHO").FirstOrDefault().CatsQuantity,
            TotalStudent = game.Students.Count,
            CurrentRound = game.Rounds.Where(x => x.Active).FirstOrDefault().CurrentRound,
            StudentDtos = game.Students.Select(x => new StudentDto
            {
                Email = x.Email,
                Name = x.Name,
                StudentCode = x.StudentCode
            })
        };

        return gameDto;
    }
}

