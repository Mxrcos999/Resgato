using Application.Dtos.Round;

namespace Application.Dtos.Game;

public class GameDto
{
    public int Id { get; set; }
    public int ProfessorId { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public IEnumerable<GetRoundDto> Round { get; set; }
}

public class PassRound
{
    public int Id { get; set; }
}
