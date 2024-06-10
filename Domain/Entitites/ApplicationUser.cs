
using Microsoft.AspNetCore.Identity;

namespace Domain.Entitites
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            
        }
        public DateTime CreateUserDate { get; set; }

        public string Name { get; set; }
        public string? StudentCode { get; set; }
        public string Type { get; set; }
        public decimal Budget { get; set; }
        public IEnumerable<Game> Games { get; set; }    
        public Professor? Professor { get; set; }
        public int? ProfessorId { get; set; }
        public IEnumerable<Settings> Setting { get; set; }
    }
}
