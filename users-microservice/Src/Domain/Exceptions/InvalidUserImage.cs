using Application.Core;

namespace User.Domain
{
    public class InvalidUserImageException : DomainException
    {
        public InvalidUserImageException() : base("Invalid user image url.") { }
    }
}
