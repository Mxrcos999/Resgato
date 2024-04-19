namespace Application.Dtos.User.Create
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string StudentCode { get; set; }
        public string PasswordConfirm { get; set; }
        public string Password { get; set; }
    }
}
