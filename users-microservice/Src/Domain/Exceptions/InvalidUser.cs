using Application.Core;

namespace User.Domain
{
    public class InvalidUserException : DomainException
    {
        public InvalidUserException() : base("Invalid user.") { }
    }
}