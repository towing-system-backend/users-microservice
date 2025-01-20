using Application.Core;

namespace User.Domain
{
    public class UserPhoneNumberUpdatedEvent(string publisherId, string type, UserPhoneNumberUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserPhoneNumberUpdated(string phoneNumber)
    {
        public readonly string PhoneNumber = phoneNumber;

        public static UserPhoneNumberUpdatedEvent CreateEvent(UserId publisherId, UserPhoneNumber phoneNumber)
        {
            return new UserPhoneNumberUpdatedEvent(
                    publisherId.GetValue(),
                    typeof(UserPhoneNumberUpdated).Name,
                    new UserPhoneNumberUpdated(
                    phoneNumber.GetValue()
                )
            );
        }
    }
}