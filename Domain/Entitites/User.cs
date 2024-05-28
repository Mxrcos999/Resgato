namespace Domain.Entitites;

public class User
{
    public User() { }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string? StudentCode { get; set; }
    public decimal Budget { get; set; }
}
