using Application.Dtos.Game;
using Application.Dtos.Prevention;
using Domain.Entitites;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;
public interface IGameService
{
    Task<bool> AddGame(GameDto dto);
    Task<IEnumerable<GameDto>> GetGame();
}

public class GameService : IGameService
{
    private readonly IBaseRepository<Game> preventionRepo;
    public GameService(IBaseRepository<Game> preventionRepo)
    {
        this.preventionRepo = preventionRepo;
    }

    public async Task<bool> AddGame(GameDto dto)
    {
        //var prevention = new Game()
        //{
        //    Professor = dto.
        //};

        //await preventionRepo.AddAsync(prevention);

        return true;
    }

    public async Task<IEnumerable<GameDto>> GetGame()
    {
        return (from prevention in await preventionRepo.GetAllAsync()
                select new GameDto()
                {
                    Id = prevention.Id,
                    Round = prevention.Rounds.Select(x => new Dtos.Round.RoundDto
                    {
                        Id = x.Id,
                        Deadline = x.Deadline
                    }),

                }).AsEnumerable();
    }
}

