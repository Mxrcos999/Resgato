using Application.Dtos.Round;

namespace Application.Dtos.Game;

public class GetGameDto
{
    public int Id { get; set; }
    public int ProfessorId { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public IEnumerable<GetRoundDto> Round { get; set; }
}
public class GameDto
{
    public IEnumerable<string> StudentsId { get; set; }
    public IEnumerable<RoundDto> Round { get; set; }
}

public class GameInformation
{
    public int Id { get; set; }
    public int currentRound { get; set; }
    public decimal BudgetUser { get; set; }
    public IEnumerable<StudentDto> StudentDtos { get; set; }
}

public class StudentDto
{
    public string Name { get; set; }
    public string StudentCode { get; set; }
    public string Email { get; set; }
}
public class PassRound
{
    public int Id { get; set; }
}
