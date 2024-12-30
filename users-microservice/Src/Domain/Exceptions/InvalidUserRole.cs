using Application.Core;

namespace User.Domain
{
    public class InvalidUserRoleException : DomainException
    {
        public InvalidUserRoleException() : base("Invalid user role.") { }
    }
}
