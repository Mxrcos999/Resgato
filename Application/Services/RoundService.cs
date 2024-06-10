using Application.Dtos.Game;
using Application.Dtos.Round;
using Application.Services.Identity;
using Domain.Entitites;
using Infrastructure.Repositories;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;
public interface IRoundService
{
    Task<bool> AddRound(RoundDto dto);
    Task<IEnumerable<RoundGet>> GetRounds();
    Task<bool> PassRound(PassRound dto);
}

public class RoundService : IRoundService
{
    private readonly IBaseRepository<Round> roundRepo;
    private readonly IGameRep gameRepo;
    private readonly IIdentityService userRep;

    public RoundService(IBaseRepository<Round> roundRepo, IIdentityService userRep, IGameRep gameRepo)
    {
        this.roundRepo = roundRepo;
        this.userRep = userRep;
        this.gameRepo = gameRepo;
    }

    public async Task<bool> AddRound(RoundDto dto)
    {
        var round = new Round()
        {
            CurrentRound = 1,
            Deadline = dto.Deadline
        };

        await roundRepo.AddAsync(round);

        return true;
    }

    public async Task<bool> PassRound(PassRound dto)
    {
        var game = (await gameRepo.GetAllGame()).Where(x => x.Id == dto.Id).FirstOrDefault();

        var currentRound = game.Rounds.Where(x => x.Active == true).FirstOrDefault();

        currentRound.Active = false;
        var nextRound = game.Rounds.Where(x => x.CurrentRound == currentRound.CurrentRound + 1).FirstOrDefault();
        nextRound.Active = true;

        for (int i = 0; i < game.Rounds.Count; i++)
        {
            if (game.Rounds.ToList()[i].CurrentRound == currentRound.CurrentRound)
            {
                game.Rounds.ToList()[i] = currentRound;
            }
            else if (nextRound != null && game.Rounds.ToList()[i].CurrentRound == nextRound.CurrentRound)
            {
                game.Rounds.ToList()[i] = nextRound;
            }
        }
        await roundRepo.UpdateAsync(currentRound);

        return false;
    }

    public async Task<IEnumerable<RoundGet>> GetRounds()
    {
        var rounds = await roundRepo.GetAllAsync();
        return rounds.Select(round => new RoundGet
        {
            Id = round.Id,
            Deadline = round.Deadline
        });
    }
}
