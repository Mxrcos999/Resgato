namespace Application.Dtos.Round;
public class RoundDto
{
    public int Id { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public DateTime Deadline { get; set; }
}

public class AnswerRoundDto
{
    public int QtdMaleCastrate { get; set; }
    public int QtdFemaleCastrate { get; set; }
    public DateTime DateCastration { get; set; }
    public int QtdMaleShelter { get; set; }
    public int QtdFamaleShelter { get; set; }
}

public class RoundGet
{
    public int Id { get; set; }
    public IEnumerable<UserResponse> Students { get; set; }
    public DateTime Deadline { get; set; }
}

public class UserResponse
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string StudentCode { get; set; }
}
