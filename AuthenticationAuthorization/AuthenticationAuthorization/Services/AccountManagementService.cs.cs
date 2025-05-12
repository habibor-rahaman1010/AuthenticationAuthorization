using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Entities;
using AuthenticationAuthorization.UnitOfWorks;
using AuthenticationAuthorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAuthorization.Services
{
    [AllowAnonymous]
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IAuthenticationUnitOfWork _authenticationUnitOfWork;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IApplicationTime _applicationTime;
        public AccountManagementService(IAuthenticationUnitOfWork authenticationUnitOfWork, IApplicationTime applicationTime)
        {
            _applicationTime = applicationTime;
            _authenticationUnitOfWork = authenticationUnitOfWork;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<string> LoginUserAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            return await _authenticationUnitOfWork.AccountRepository.LoginAsync(request, cancellationToken);
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDto request, CancellationToken cancellationToken = default)
        {
            var user = new User();
            user.Id = Guid.NewGuid();
            user.FistName = request.FistName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.Phone = request.Phone;
            user.Role = request.Role;
            user.Address = request.Address;
            user.Gender = request.Gender;
            user.Nationality = request.Nationality;
            user.Email = request.Email;
            user.PasswordHashed = _passwordHasher.HashPassword(user, request.Password);
            user.CreatedDate = _applicationTime.GetCurrentTime();
            user.LastModifiedDate = _applicationTime.GetCurrentTime();

            await _authenticationUnitOfWork.AccountRepository.RegisterAsync(user, cancellationToken);
            await _authenticationUnitOfWork.SaveAsync();
            return user;
        }
    }
}
