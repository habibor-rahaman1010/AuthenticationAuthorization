using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountManagementService _accountManagementService;
        private readonly IUserManagementService _userManagementService;

        public AuthController(IAccountManagementService accountManagementService,
            IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
            _accountManagementService = accountManagementService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRegistrationDto request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existUser = await _userManagementService.UserExistAsync(request.Email);
            if (existUser == true)
            {
                return BadRequest(new {message = $"User already exist by this email: {request.Email}"});
            }
            var user = await _accountManagementService.RegisterUserAsync(request, cancellationToken);

            var response = new UserResponseDto
            {
                Id = user.Id,
                FistName = user.FistName,
                LastName = user.LastName,
                UserName = user.UserName,
                Phone = user.Phone,
                Role = user.Role,
                Address = user.Address,
                Gender = user.Gender,
                Nationality = user.Nationality,
                Email = user.Email,
                PasswordHashed = user.PasswordHashed,
                CreatedDate = user.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                LastModifiedDate = user.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseDto>> Login(UserLoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Validation failed!" });
            }
            var response = await _accountManagementService.LoginUserAsync(request);
            if (response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid user email or password!"
                });
            }

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserLoginResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _accountManagementService.RefreshTokenAsync(request);
            if (result == null || result.AccessToken == null || result.RefreshToken == null || result.Email == null)
            {
                return Unauthorized("Invalid Refresh Token");
            }
            return Ok(result);
        }
    }
}
