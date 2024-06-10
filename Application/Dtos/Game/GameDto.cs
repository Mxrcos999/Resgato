using Application.Dtos.Round;

namespace Application.Dtos.Game;

public class GameDto
{
    public int Id { get; set; }
    public int ProfessorId { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public IEnumerable<RoundDto> Round { get; set; }
}
