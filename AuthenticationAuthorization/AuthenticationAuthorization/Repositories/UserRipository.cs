using AuthenticationAuthorization.DatabaseContext;
using AuthenticationAuthorization.Entities;

namespace AuthenticationAuthorization.Repositories
{
    public class UserRipository : Repository<User, Guid>, IUserRepository
    {
        public UserRipository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }    
    }
}
