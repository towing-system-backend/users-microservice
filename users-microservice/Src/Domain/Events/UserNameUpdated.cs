using Application.Core;

namespace User.Domain
{
    public class UserNameUpdatedEvent(string publisherId, string type, UserNameUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserNameUpdated(string name)
    {
        public readonly string Name = name;

        public static UserNameUpdatedEvent CreateEvent(UserId publisherId, UserName userName)
        {
            return new UserNameUpdatedEvent(
                publisherId.GetValue(),
                typeof(UserNameUpdated).Name,
                new UserNameUpdated(
                    userName.GetValue()
                )
            );
        }
    }
}