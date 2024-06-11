﻿using Application.Dtos.Game;
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
}

public class GameService : IGameService
{
    private readonly IBaseRepository<Game> gameRep;
    private readonly IBaseRepository<Professor> professorRep;
    private readonly IBaseRepository<Answers> resultRep;
    private readonly ISettingRep settingRep;
    private readonly IGameRep gameRepo;
    private readonly IIdentityService userRep;

    public GameService(IBaseRepository<Game> gameRep, IBaseRepository<Professor> professorRep, IIdentityService userRep, IGameRep gameRepo, ISettingRep settingRep, IBaseRepository<Answers> resultRep)
    {
        this.gameRep = gameRep;
        this.professorRep = professorRep;
        this.userRep = userRep;
        this.gameRepo = gameRepo;
        this.settingRep = settingRep;
        this.resultRep = resultRep;
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

        return true;
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
        var budget = await userRep.GetBudgetAsync();

        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == id).FirstOrDefault();
        if (game is null)
            return null;

        var settingList = (await settingRep.GetAllByIdAsync()).ToList();
        var currentRound = game.Rounds.Where(x => x.Active).FirstOrDefault().CurrentRound;

        var answarRound = (await GetResultsAsync(id)).Rounds.Where(x => x.RoundNumber == currentRound).FirstOrDefault();
        var answared = answarRound is null ? false : true;

        var gameDto = new GameInformation()
        {
            BudgetUser = budget,
            GameName = game.GameName,
            AnsweredRound = answared,
            GameConcluded = game.Concluded,
            Id = id,
            TotalCatsFemale = settingList.Where(x => x.Gender == "Femea").FirstOrDefault().CatsQuantity,
            TotalCatsMale = settingList.Where(x => x.Gender == "MACHO").FirstOrDefault().CatsQuantity,
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
}

