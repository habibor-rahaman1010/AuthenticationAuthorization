using AuthenticationAuthorization.Entities;
using AuthenticationAuthorization.UnitOfWorks;

namespace AuthenticationAuthorization.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IAuthenticationUnitOfWork _authenticationUnitOfWork;
        public UserManagementService(IAuthenticationUnitOfWork authenticationUnitOfWork)
        {
            _authenticationUnitOfWork = authenticationUnitOfWork;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _authenticationUnitOfWork.UserRepository.GetByIdAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            await _authenticationUnitOfWork.UserRepository.DeleteAsync(user);
            await _authenticationUnitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _authenticationUnitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _authenticationUnitOfWork.UserRepository.GetByEmailAsync(e => e.Email == email);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _authenticationUnitOfWork.UserRepository.GetByIdAsync(u => u.Id == userId);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _authenticationUnitOfWork.UserRepository.UpdateAsync(user);
            await _authenticationUnitOfWork.SaveAsync();
        }

        public async Task<bool> UserExistAsync(string email)
        {
            return await _authenticationUnitOfWork.UserRepository.ExistsAsync(x => x.Email == email);
        }
    }
}
