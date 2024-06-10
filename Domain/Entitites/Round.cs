namespace Domain.Entitites;

public sealed class Round
{
    public int Id { get; set; }
    public DateTime Deadline { get; set; }
    public int CurrentRound { get; set; }
    public bool Active { get; set; }
}
