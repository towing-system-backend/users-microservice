using Application.Core;

namespace User.Application
{
    public class UserAlreadyExistsError : ApplicationError
    {
        public UserAlreadyExistsError() : base("User already exists.") { }
    }
}