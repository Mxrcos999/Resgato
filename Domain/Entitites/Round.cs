namespace Domain.Entitites;

public sealed class Round
{
    public int Id { get; set; }
    public List<ApplicationUser> Students { get; set; }
    public ApplicationUser Professor { get; set; }
    public DateTime Deadline { get; set; }
    public int CurrentRound { get; set; }
}
