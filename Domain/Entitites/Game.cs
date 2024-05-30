namespace Domain.Entitites;

public sealed class Game
{
    public int Id { get; set; }
    public ICollection<ApplicationUser> Students { get; set; }

    public Professor Professor { get; set; }
    public ICollection<Round> Rounds { get; set; }
}
