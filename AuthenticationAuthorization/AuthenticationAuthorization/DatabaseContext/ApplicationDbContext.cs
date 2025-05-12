using AuthenticationAuthorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
          
        }

        public DbSet<User> Users { get; set; }
    }
}
