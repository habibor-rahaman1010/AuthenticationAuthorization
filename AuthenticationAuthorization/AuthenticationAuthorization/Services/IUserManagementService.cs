using AuthenticationAuthorization.Entities;

namespace AuthenticationAuthorization.Services
{
    public interface IUserManagementService
    {
        public Task<bool> UserExistAsync(string email);
        public Task<IEnumerable<User>> GetAllUserAsync();
        public Task<User> GetUserByIdAsync(Guid userId);
        public Task<User> GetUserByEmailAsync(string email);
        public Task UpdateUserAsync(User user);
        public Task<bool> DeleteUserAsync(Guid userId);
    }
} 
