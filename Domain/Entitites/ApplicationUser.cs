
using Microsoft.AspNetCore.Identity;

namespace Domain.Entitites
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreateUserDate { get; set; }

        public string Name { get; set; }
        public string StudentCode { get; set; }
        public string Type { get; set; }
    }
}
