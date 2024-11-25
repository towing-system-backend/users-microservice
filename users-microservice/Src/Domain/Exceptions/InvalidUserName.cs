using Application.Core;

namespace User.Domain
{
    public class InvalidUserNameException : DomainException
    {
        public InvalidUserNameException() : base("Invalid user name.") { }
    }
}