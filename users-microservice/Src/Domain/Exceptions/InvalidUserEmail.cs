using Application.Core;

namespace User.Domain
{
    public class InvalidUserEmailException : DomainException
    {
        public InvalidUserEmailException() : base("Invalid user email.") { }
    }
}