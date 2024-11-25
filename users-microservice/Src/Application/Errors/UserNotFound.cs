using Application.Core;

namespace User.Application
{
    public class UserNotFoundError : ApplicationError
    {
        public UserNotFoundError() : base("User not found.") { }
    }
}