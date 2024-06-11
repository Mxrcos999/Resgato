using Application.Dtos.Round;
using Domain.Entitites;
using Infrastructure.Context;
using Infrastructure.Repositories.BaseRepository;
using System.Security.Claims;

namespace Application.Services;

public interface IAnswersService
{
    Task<bool> CreateAnswer(AnswerRoundDto dto, int currentRound, ResultRound resultRound, DateTime deadLine);
}
public class AnswersService : IAnswersService
{
    private readonly IBaseRepository<Answers> baseRepository;
    private readonly ApplicationContext _context;
    private readonly string _userId;

    public AnswersService(IBaseRepository<Answers> baseRepository, ApplicationContext context)
    {
        this.baseRepository = baseRepository;
        _context = context;
        _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    }
    public async Task<bool> CreateAnswer(AnswerRoundDto dto, int currentRound, ResultRound resultRound, DateTime deadLine)
    {
        var answer = new Answers()
        {
            Round = currentRound,
            ApplicationUserId = _userId,
            DateCastration = dto.DateCastration,
            QuantityFemaleCastrate = dto.QtdFemaleCastrate,
            QuantityMaleCastrate = dto.QtdMaleCastrate,
            DeadLine = deadLine,
            QuantityFemaleShelter = dto.QtdFamaleShelter,
            QuantityMaleShelter = dto.QtdMaleShelter,
            GameId = dto.GameId,
            TotalPopulation = resultRound.TotalPopulation,
            TotalPopulationCastrated = resultRound.TotalPopulationCastrated,
            TotalPopulationFemaleCastrated = resultRound.TotalPopulationCastrated,
            TotalPopulationMaleCastrated = resultRound.TotalPopulationMaleCastrated,
        };

        await baseRepository.AddAsync(answer);

        return true;
    }
}
