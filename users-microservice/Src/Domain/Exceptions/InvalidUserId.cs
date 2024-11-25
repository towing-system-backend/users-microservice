using Application.Core;

namespace User.Domain
{
    public class InvalidUserIdException : DomainException
    {
        public InvalidUserIdException() : base("Invalid user id.") { }
    }
}