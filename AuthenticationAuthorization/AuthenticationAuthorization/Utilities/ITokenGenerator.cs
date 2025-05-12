using AuthenticationAuthorization.Entities;

namespace AuthenticationAuthorization.Utilities
{
    public interface ITokenGenerator
    {
        public string CreateJwtAuthenticationToken(User user);
    }
}
