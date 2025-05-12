using AuthenticationAuthorization.Repositories;

namespace AuthenticationAuthorization.UnitOfWorks
{
    public interface IAuthenticationUnitOfWork : IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public IUserRepository UserRepository { get; }
    }
}
