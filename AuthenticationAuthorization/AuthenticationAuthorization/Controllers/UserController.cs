using AuthenticationAuthorization.Dtos;
using AuthenticationAuthorization.Services;
using AuthenticationAuthorization.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAuthorization.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ApiBaseController
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IApplicationTime _applicationTime;
        public UserController(IUserManagementService userManagementService, IApplicationTime applicationTime)
        {
            _applicationTime = applicationTime;
            _userManagementService = userManagementService;
        }

        [HttpGet("authenticated")]
        public IActionResult Authenticated()
        {
            return Ok(new { message = "Here user all information and this rout is access can only authenticadted users!" });
        }

        [HttpGet("getallusers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUserAsync()
        {
            var user = await _userManagementService.GetAllUserAsync();
            if (user.Count() == 0)
            {
                return Ok(new { meassage = "The user list is empty!" });
            }
            return Ok(user);
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(Guid id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null) {
                return BadRequest(new { message = "User not found!" });
            }
            var response = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
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

        [HttpGet("GetUserByEmailId")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
        {
            var user = await _userManagementService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found!" });
            }
            var response = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
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

        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser([FromBody] UpdateUserDto request, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid");
            }

            var existUser = await _userManagementService.GetUserByIdAsync(id);
            if (existUser == null)
            {
                return BadRequest(new { messaage = "User not found in your system!" });
            }

            existUser.FirstName = request.FirstName;
            existUser.LastName = request.LastName;
            existUser.UserName = request.UserName;
            existUser.Phone = request.Phone;
            existUser.Role = request.Role;
            existUser.Address = request.Address;
            existUser.Gender = request.Gender;
            existUser.Nationality = request.Nationality;
            existUser.Email = request.Email;
            existUser.LastModifiedDate = _applicationTime.GetCurrentTime();
            await _userManagementService.UpdateUserAsync(existUser);

            var response = new UserResponseDto
            {
                Id = existUser.Id,
                FirstName = existUser.FirstName,
                LastName = existUser.LastName,
                UserName = existUser.UserName,
                Phone = existUser.Phone,
                Role = existUser.Role,
                Address = existUser.Address,
                Gender = existUser.Gender,
                Nationality = existUser.Nationality,
                Email = existUser.Email,
                PasswordHashed = existUser.PasswordHashed,
                CreatedDate = existUser.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                LastModifiedDate = existUser.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var isdeleted = await _userManagementService.DeleteUserAsync(id);
            if (!isdeleted)
            {
                return BadRequest(new { message = "User not found so can not deleted!" });
            }
            return Ok(new {message = "User succesfully deleted!"});
        }
    }
}
