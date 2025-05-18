using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Entities;

namespace AuthenticationAuthorization.Services
{
    public interface IAccountManagementService
    {
        public Task<User> RegisterUserAsync(UserRegistrationDto request, CancellationToken cancellationToken = default);
        public Task<UserLoginResponseDto> LoginUserAsync(UserLoginDto request, CancellationToken cancellationToken = default);
        public Task<UserLoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
