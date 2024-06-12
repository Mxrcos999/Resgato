namespace Domain.Entitites;

public sealed class Settings
{
    public Settings()
    {
        
    }

    public Game Game { get; set; }
    public int GameId { get; set; }
    public int Id { get; set; }
    public decimal BudgetGame { get; set; }
    public ICollection<SettingCat> SettingCat { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserId { get; set; }
}

public class SettingCat
{
    public int Id { get; set; }
    public int CatsQuantity { get; set; }
    public string Gender { get; set; }
}