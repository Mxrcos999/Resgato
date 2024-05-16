using Domain.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public readonly IHttpContextAccessor _contextAccessor;
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor contextAccessor)
            : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        public DbSet<PreventionAction> Actions { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Round> Round { get; set; }
    }
}
