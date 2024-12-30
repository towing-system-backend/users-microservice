using Application.Core;

namespace User.Domain
{
    public class InvalidUserPhoneNumberException : DomainException
    {
        public InvalidUserPhoneNumberException() : base("Invalid user phone number.") { }
    }
}
