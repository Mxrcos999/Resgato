namespace Domain.Entitites;

public sealed class Game
{
    public int Id { get; set; }
    public ICollection<ApplicationUser> Students { get; set; }
    public string GameName { get; set; }
    public string ProfessorEmail { get; set; }
    public ICollection<Round> Rounds { get; set; }
    public Settings? Settings { get; set; }
    public int? SettingId { get; set; }
    public bool Concluded { get; set; }
}
