using AuthenticationAuthorization.DatabaseContext;
using AuthenticationAuthorization.Repositories;

namespace AuthenticationAuthorization.UnitOfWorks
{
    public class AuthenticationUnitOfWork : UnitOfWork, IAuthenticationUnitOfWork
    {
        public IAccountRepository AccountRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public AuthenticationUnitOfWork(ApplicationDbContext applicationDbContext,
            IUserRepository userRepository,
            IAccountRepository accountRepository) : base(applicationDbContext)
        {
            AccountRepository = accountRepository;
            UserRepository = userRepository;
        }
    }
}
