using Application.Core;

namespace User.Domain
{
    public class UserIdentificationNumberUpdatedEvent(string publisherId, string type, UserIdentificationNumberUpdated context) : DomainEvent(publisherId, type, context) { }

    public class UserIdentificationNumberUpdated(int identificationNumber)
    {
        public readonly int IdentificationNumber = identificationNumber;

        public static UserIdentificationNumberUpdatedEvent CreateEvent(UserId publisherId, UserIdentificationNumber identificationNumber)
        {
            return new UserIdentificationNumberUpdatedEvent(
                    publisherId.GetValue(),
                    typeof(UserIdentificationNumberUpdated).Name,
                    new UserIdentificationNumberUpdated(
                    identificationNumber.GetValue()
                )
            );
        }
    }
}