using Application.Core;

namespace User.Domain
{
    public class InvalidUserStatusException: DomainException
    {
        public InvalidUserStatusException() : base("Invalid user status.") { }
    }
}
