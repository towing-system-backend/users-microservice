using Application.Core;

namespace User.Domain
{
    public class UserRoleUpdatedEvent(string publisherId, string type, UserRoleUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserRoleUpdated(string role)
    {
        public readonly string Role = role;

        public static UserRoleUpdatedEvent CreateEvent(UserId publisherId, UserRole role)
        {
            return new UserRoleUpdatedEvent(
                    publisherId.GetValue(),
                    typeof(UserRoleUpdated).Name,
                    new UserRoleUpdated(
                    role.GetValue()
                )
            );
        }
    }
}