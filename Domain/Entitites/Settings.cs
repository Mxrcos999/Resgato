namespace Domain.Entitites;

public sealed class Settings
{
    public Settings()
    {
        
    }

    public int Id { get; set; }
    public int CatsQuantity { get; set; }
    public string Gender { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
