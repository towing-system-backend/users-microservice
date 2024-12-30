using Application.Core;

namespace User.Domain
{
    public class UserImageUpdatedEvent(string publisherId, string type, UserImageUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserImageUpdated(string image)
    {
        public readonly string Image = image;

        public static UserImageUpdatedEvent CreateEvent(UserId publisherId, UserImage image)
        {
            return new UserImageUpdatedEvent(
                    publisherId.GetValue(),
                    typeof(UserImageUpdated).Name,
                    new UserImageUpdated(
                    image.GetValue()
                )
            );
        }
    }
}