namespace Application.Dtos.Round;
public class RoundDto
{
    public DateTime Deadline { get; set; }
}

public class GetRoundDto
{
    public int Id { get; set; }
    public DateTime Deadline { get; set; }
    public int CurrentRound { get; set; }
    public bool Active { get; set; }
}

public class AnswerRoundDto
{
    public int GameId { get; set; }
    public int QtdMaleCastrate { get; set; }
    public int QtdFemaleCastrate { get; set; }
    public DateTime DateCastration { get; set; }
    public int QtdMaleShelter { get; set; }
    public int QtdFamaleShelter { get; set; }
}

public class RoundGet
{
    public int Id { get; set; }
    public DateTime Deadline { get; set; }
}

public class UserResponse
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string StudentCode { get; set; }
}
