using Orchard.Security;
using Orchard.Users.Events;

namespace ceenq.com.Users.Events
{
    public class UserEventHandler : IUserEventHandler
    {
        public void Creating(UserContext context)
        {

        }

        public void Created(UserContext context)
        {

        }

        public void LoggingIn(string userNameOrEmail, string password)
        {
        }

        public void LoggedIn(IUser user)
        {
        }

        public void LogInFailed(string userNameOrEmail, string password)
        {
        }

        public void LoggedOut(IUser user)
        {
        }

        public void AccessDenied(IUser user)
        {
        }

        public void ChangedPassword(IUser user)
        {
        }

        public void SentChallengeEmail(IUser user)
        {
        }

        public void ConfirmedEmail(IUser user)
        {
        }

        public void Approved(IUser user)
        {
        }
    }
}