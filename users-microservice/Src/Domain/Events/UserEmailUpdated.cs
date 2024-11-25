using Application.Core;

namespace User.Domain
{
    public class UserEmailUpdatedEvent(string publisherId, string type, UserEmailUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserEmailUpdated(string email)
    {
        public readonly string Email = email;

        static public UserEmailUpdatedEvent CreateEvent(UserId publisherId, UserEmail email)
        {
            return new UserEmailUpdatedEvent(
                publisherId.GetValue(),
                typeof(UserEmailUpdated).Name,
                new UserEmailUpdated(
                    email.GetValue()
                )
            );
        }
    }
}