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

public class ResultRound
{
    public int TotalPopulation { get; set; }
    public int TotalPopulationCastrated { get; set; }
    public int TotalPopulationFemaleCastrated { get; set; }
    public int TotalPopulationMaleCastrated { get; set; }
}

public class GetGameResult 
{
    public string Name { get; set; }
    public ICollection<GetRoundResult> Rounds { get; set;}
}

public class GetRoundResult
{
    public int RoundNumber { get; set; }
    public DateTime DeadLine { get; set; }
    public int QtdMaleCastrate { get; set; }
    public int QtdFemaleCastrate { get; set; }
    public DateTime DateCastration { get; set; }
    public int QtdMaleShelter { get; set; }
    public int QtdFamaleShelter { get; set; }
    public ResultRound ResultRound { get; set; }
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
