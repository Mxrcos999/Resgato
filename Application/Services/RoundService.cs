using Application.Dtos.Round;
using Application.Services.Identity;
using Domain.Entitites;
using Infrastructure.Repositories.BaseRepository;

namespace Application.Services;
public interface IRoundService
{
    Task<bool> AddRound(RoundDto dto);
    Task<IEnumerable<RoundGet>> GetRounds();
}

public class RoundService : IRoundService
{
    private readonly IBaseRepository<Round> roundRepo;
    private readonly IIdentityService userRep;

    public RoundService(IBaseRepository<Round> roundRepo, IIdentityService userRep)
    {
        this.roundRepo = roundRepo;
        this.userRep = userRep;
    }

    public async Task<bool> AddRound(RoundDto dto)
    {
        var students = await  userRep.GetStudents(dto.StudentsId);

        var round = new Round()
        {
            Id = dto.Id,
            Students = students,
            Deadline = dto.Deadline
        };

        await roundRepo.AddAsync(round);

        return true;
    }

    public async Task<bool> PassRound(RoundDto dto)
    {
        var students = await  userRep.GetStudents(dto.StudentsId);

        var round = new Round() 
        {
            Id = dto.Id,
            Students = students,
            Deadline = dto.Deadline
        };

        await roundRepo.AddAsync(round);

        return true;
    }

    public async Task<IEnumerable<RoundGet>> GetRounds()
    {
        var rounds = await roundRepo.GetAllAsync();
        return rounds.Select(round => new RoundGet
        {
            Id = round.Id,
            Students = (from student in round.Students select new UserResponse()
            {
                Id = student.Id, 
                Nome = student.Name, 
                StudentCode = student.StudentCode 
            }).AsEnumerable(),
            Deadline = round.Deadline
        });
    }
}
