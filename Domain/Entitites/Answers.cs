namespace Domain.Entitites;

public class Answers
{
    public int Id { get; set; }
    public Game Game { get; set; }
    public int GameId { get; set; }
    public int Round { get; set; }  
    public ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserId { get; set; }
    public DateTime DateCastration { get; set; }
    public DateTime DeadLine { get; set; }
    public int QuantityMaleCastrate { get; set; }
    public int QuantityFemaleCastrate { get; set; }
    public int QuantityFemaleShelter { get; set; }
    public int QuantityMaleShelter { get; set; }
    public int TotalPopulation { get; set; }
    public int TotalPopulationCastrated { get; set; }
    public int TotalPopulationFemaleCastrated { get; set; }
    public int TotalPopulationMaleCastrated { get; set; }
}
