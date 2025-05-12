
namespace AuthenticationAuthorization.Utilities
{
    public class ApplicationTime : IApplicationTime
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
