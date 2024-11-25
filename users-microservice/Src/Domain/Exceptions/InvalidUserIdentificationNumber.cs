using Application.Core;

namespace User.Domain
{
    public class InvalidUserIdentificationNumberException : DomainException
    {
        public InvalidUserIdentificationNumberException() : base("Invalid user identification number.") { }
    }
}