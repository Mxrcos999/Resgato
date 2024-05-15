namespace Application.Dtos.User.Create
{
    public class CreateStudentUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string StudentCode { get; set; }
        public string PasswordConfirm { get; set; }
        public string Password { get; set; }
    }  
    
    public class CreateProfessorUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordConfirm { get; set; }
        public string Password { get; set; }
    }
}
