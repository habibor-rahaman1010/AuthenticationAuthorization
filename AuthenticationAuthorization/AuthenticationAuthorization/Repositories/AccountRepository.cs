using AuthenticationAuthorization.DatabaseContext;
using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Entities;
using AuthenticationAuthorization.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAuthorization.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        public AccountRepository(ApplicationDbContext dbContext, ITokenGenerator tokenGenerator)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public Task<bool> AssignRoleAsync(string email, string role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User> LoginAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
            if(user == null)
            {
                return null;
            }
            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            
            return user;
        }

        public Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterAsync(User request, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(request, cancellationToken);
        }
    }
}
