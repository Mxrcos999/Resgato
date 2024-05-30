namespace Domain.Entitites;

public class Professor
{
    public int Id { get; set; }
    public ICollection<Game>? Games { get; set; }
    public ApplicationUser User { get; set; }
}
