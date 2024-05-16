namespace Domain.Entitites;

public sealed class PreventionAction
{
    public PreventionAction()
    {
        
    }
    public int Id { get; set; }
    public string Action { get; set; }
    public decimal Value { get; set; }
    public string Gender { get; set; }
}
