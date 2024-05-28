namespace Application.Dtos.Round;
public class RoundDto
{
    public int Id { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public DateTime Deadline { get; set; }
}

public class RoundGet
{
    public int Id { get; set; }
    public IEnumerable<UserResponse> Students { get; set; }
    public DateTime Deadline { get; set; }
}

public class UserResponse
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public string StudentCode { get; set; }
}
