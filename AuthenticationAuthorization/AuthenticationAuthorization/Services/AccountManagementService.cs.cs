using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Entities;
using AuthenticationAuthorization.UnitOfWorks;
using AuthenticationAuthorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace AuthenticationAuthorization.Services
{
    [AllowAnonymous]
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IAuthenticationUnitOfWork _authenticationUnitOfWork;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IApplicationTime _applicationTime;
        private readonly ITokenGenerator _tokenGenerator;

        public AccountManagementService(IAuthenticationUnitOfWork authenticationUnitOfWork, 
            ITokenGenerator tokenGenerator,
            IApplicationTime applicationTime)
        {
            _applicationTime = applicationTime;
            _tokenGenerator = tokenGenerator;
            _authenticationUnitOfWork = authenticationUnitOfWork;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto request, CancellationToken cancellationToken = default)
        {
            var user = await _authenticationUnitOfWork.AccountRepository.LoginAsync(request, cancellationToken);
            return await CreateResponseToken(user);
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15);
            await _authenticationUnitOfWork.SaveAsync();
            return refreshToken;
        }

        public async Task<UserLoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
           var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user == null)
            {
                return null;
            }
            return await CreateResponseToken(user);
        }

        private async Task<User> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await _authenticationUnitOfWork.UserRepository.GetByIdAsync(x => x.Id == userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= _applicationTime.GetCurrentTime())
            {
                return null;
            }
            return user;
        }

        private async Task<UserLoginResponseDto> CreateResponseToken(User user)
        {
            return new UserLoginResponseDto
            {
                AccessToken = _tokenGenerator.CreateJwtAuthenticationToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user),
                Email = user.Email
            };
        }
    }
}
