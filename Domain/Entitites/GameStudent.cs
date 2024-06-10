namespace Domain.Entitites;

public class GameStudent
{
    public int Id { get; set; }
    public Game Game { get; set; }
    public int GameId { get; set; }  
    public ApplicationUser Student { get; set; }
    public int StudentId { get; set; }
}
