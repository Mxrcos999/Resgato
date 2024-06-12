using Application.Dtos.Game;
using Application.Dtos.Round;
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
    Task<IEnumerable<Players>> GetPlayers(int id);
    Task<GetGameResult> GetResultsAsync(int gameId);
    Task<bool> FinishGame(int id);
    Task<GetGameResult> GetResultsByIdAsync(int gameId, string userId);
}

public class GameService : IGameService
{
    private readonly IBaseRepository<Game> gameRep;
    private readonly IBaseRepository<Settings> settingsRep;
    private readonly IBaseRepository<SettingCat> settingCatRep;
    private readonly IBaseRepository<Professor> professorRep;
    private readonly IBaseRepository<Answers> resultRep;
    private readonly ISettingRep settingRep;
    private readonly IGameRep gameRepo;
    private readonly IIdentityService userRep;

    public GameService(IBaseRepository<Game> gameRep, IBaseRepository<Professor> professorRep, IIdentityService userRep, IGameRep gameRepo, ISettingRep settingRep, IBaseRepository<Answers> resultRep, IBaseRepository<Settings> settingsRep, IBaseRepository<SettingCat> settingCatRep)
    {
        this.gameRep = gameRep;
        this.professorRep = professorRep;
        this.userRep = userRep;
        this.gameRepo = gameRepo;
        this.settingRep = settingRep;
        this.resultRep = resultRep;
        this.settingsRep = settingsRep;
        this.settingCatRep = settingCatRep;
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
            if (i + 1 == 1)
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
            GameName = dto.GameName,
            ProfessorEmail = professor,
            Rounds = rounds,
            Students = students,
        };

        await gameRep.AddAsync(game);
        CreateSetting(dto, game);
     
        return true;
    }

    private void CreateSetting(GameDto dto, Game game)
    {
        Settings setting = new Settings();
        foreach(var item in dto.StudentsId)
        {
            setting = new Settings()
            {
                BudgetGame = 10000m,
                Game = game,
                GameId = game.Id,
                ApplicationUserId = item,
                SettingCat = new List<SettingCat>()
                {
                    new SettingCat()
                    {
                        CatsQuantity = 200,
                        Gender = "Macho"
                    },
                    new SettingCat()
                    {
                        CatsQuantity = 200,
                        Gender = "Femea"
                    },
                }
            };
        }

        settingsRep.AddAsync(setting);
    }

    public async Task<bool> FinishGame(int id)
    {
        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == id).FirstOrDefault();

        game.Concluded = true;  

        await gameRep.UpdateAsync(game);

        return true;
    }

    public async Task<IEnumerable<GetGameDto>> GetGame()
    {
        var game = from gamee in await gameRepo.GetAllGame()
                   select new GetGameDto()
                   {
                       Id = gamee.Id,
                       TotalStudent = gamee.Students.Count,
                       GameName = gamee.GameName,
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

        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == id).FirstOrDefault();

        var gameSettingsCat = (await settingCatRep.GetAllAsync()).Where(x => x.SettingsId == game.Settings.Id);
        if (game is null)
            return null;

        var settingList = (await settingRep.GetAllByIdAsync(id)).FirstOrDefault();
        var budget = settingList.BudgetGame;

        var currentRound = game.Rounds.Where(x => x.Active == true).FirstOrDefault().CurrentRound;

        var answarRound = (await GetResultsAsync(id)).Rounds;
        var answared = answarRound is null ? false : true;
        var totalMaleCastrate = answarRound.Sum(x => x.QtdMaleCastrate);
        var totalFemaleCastrate = answarRound.Sum(x => x.QtdFemaleCastrate);
        var Male = gameSettingsCat.Where(x => x.Gender == "Macho").Select(x => x.CatsQuantity).FirstOrDefault();
        var Female = gameSettingsCat.Where(x => x.Gender == "Femea").Select(x => x.CatsQuantity).FirstOrDefault();

        var gameDto = new GameInformation()
        {
            BudgetUser = budget,
            GameName = game.GameName,
            AnsweredRound = answared,
            GameConcluded = game.Concluded,
            Id = id,
            RoundActive = currentRound,
            TotalCatsFemaleCastrated = totalFemaleCastrate,
            TotalCatsMaleCastrated = totalMaleCastrate,
            TotalCatsFemale = Female - totalFemaleCastrate,
            TotalCatsMale = Male - totalMaleCastrate,
            TotalStudent = game.Students.Count,
            CurrentRound = currentRound,
            StudentDtos = game.Students.Select(x => new StudentDto
            {
                Email = x.Email,
                Name = x.Name,
                StudentCode = x.StudentCode
            })
        };

        return gameDto;
    }

    public async Task<IEnumerable<Players>> GetPlayers(int id)
    {
        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == id).FirstOrDefault();

        var result = (from student in game.Students
                      select new Players()
                      {
                          Name = student.Name,
                          TotalPopulation = 400,


                      }).ToList();

        for (int i = 0; i < result.Count(); i++)
        {
            result[i].Position = i + 1;
        }

        return result;
    }

    public async Task<GetGameResult> GetResultsAsync(int gameId)
    {
        var game = (await resultRep.GetAllAsync()).Where(x => x.GameId == gameId)
            .OrderBy(x => Math.Abs((x.DeadLine - DateTime.Today).Ticks))
            .ToList();

        var gameResult = new GetGameResult();
        gameResult.Rounds = new List<GetRoundResult>();

        foreach (var currentRound in game)
        {
            var roundResult = new GetRoundResult()
            {
                QtdMaleShelter = currentRound.QuantityMaleShelter,
                QtdMaleCastrate = currentRound.QuantityMaleCastrate,
                QtdFemaleCastrate = currentRound.QuantityFemaleCastrate,
                DeadLine = currentRound.DeadLine,
                QtdFamaleShelter = currentRound.QuantityFemaleShelter,
                DateCastration = currentRound.DateCastration,
                RoundNumber = currentRound.Round,
                ResultRound = new ResultRound()
                {
                    TotalPopulation = currentRound.TotalPopulation,
                    TotalPopulationCastrated = currentRound.TotalPopulationCastrated,
                    TotalPopulationFemaleCastrated = currentRound.TotalPopulationFemaleCastrated,
                    TotalPopulationMaleCastrated = currentRound.TotalPopulationMaleCastrated
                }
            };
            gameResult.Rounds.Add(roundResult);
        }

        gameResult.Rounds.OrderBy(x => Math.Abs((x.DeadLine - DateTime.Today).Ticks));

        return gameResult;
    } 
    
    public async Task<GetGameResult> GetResultsByIdAsync(int gameId, string userId)
    {
        var game = (await resultRep.GetAllAsync()).Where(x => x.GameId == gameId && x.ApplicationUserId == userId)
            .OrderBy(x => Math.Abs((x.DeadLine - DateTime.Today).Ticks))
            .ToList();

        var gameResult = new GetGameResult();
        gameResult.Rounds = new List<GetRoundResult>();

        foreach (var currentRound in game)
        {
            var roundResult = new GetRoundResult()
            {
                QtdMaleShelter = currentRound.QuantityMaleShelter,
                QtdMaleCastrate = currentRound.QuantityMaleCastrate,
                QtdFemaleCastrate = currentRound.QuantityFemaleCastrate,
                DeadLine = currentRound.DeadLine,
                QtdFamaleShelter = currentRound.QuantityFemaleShelter,
                DateCastration = currentRound.DateCastration,
                RoundNumber = currentRound.Round,
                ResultRound = new ResultRound()
                {
                    TotalPopulation = currentRound.TotalPopulation,
                    TotalPopulationCastrated = currentRound.TotalPopulationCastrated,
                    TotalPopulationFemaleCastrated = currentRound.TotalPopulationFemaleCastrated,
                    TotalPopulationMaleCastrated = currentRound.TotalPopulationMaleCastrated
                }
            };
            gameResult.Rounds.Add(roundResult);
        }

        gameResult.Rounds.OrderBy(x => Math.Abs((x.DeadLine - DateTime.Today).Ticks));

        return gameResult;
    }
}

