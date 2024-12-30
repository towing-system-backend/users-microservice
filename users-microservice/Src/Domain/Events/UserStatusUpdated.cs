using Application.Core;

namespace User.Domain
{
    public class UserStatusUpdatedEvent(string publisherId, string type, UserStatusUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserStatusUpdated(string status)
    {
        public readonly string Status = status;

        public static UserStatusUpdatedEvent CreateEvent(UserId publisherId, UserStatus status)
        {
            return new UserStatusUpdatedEvent(
                    publisherId.GetValue(),
                    typeof(UserStatusUpdated).Name,
                    new UserStatusUpdated(
                    status.GetValue()
                )
            );
        }
    }
}