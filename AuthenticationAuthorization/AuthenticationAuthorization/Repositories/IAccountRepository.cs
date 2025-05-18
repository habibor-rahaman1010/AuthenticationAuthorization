using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Entities;

namespace AuthenticationAuthorization.Repositories
{
    public interface IAccountRepository
    {
        public Task RegisterAsync(User request, CancellationToken cancellationToken = default);
        public Task<User> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default);
        public Task LogoutAsync(CancellationToken cancellationToken = default);
        public Task<bool> AssignRoleAsync(string email, string role, CancellationToken cancellationToken = default);
    }
}
