using Application.Core;

namespace User.Domain
{
    public class UserCreatedEvent(string publisherId, string type, UserCreated context) : DomainEvent(publisherId, type, context) { }

    public class UserCreated(string name, int identificationNumber, string email)
    {
        public readonly string Name = name;
        public readonly int IdentificationNumber = identificationNumber;
        public readonly string Email = email;

        static public UserCreatedEvent CreateEvent(UserId publisherId, UserName name, UserIdentificationNumber identificationNumber, UserEmail email)
        {
            return new UserCreatedEvent(
                publisherId.GetValue(),
                typeof(UserCreated).Name,
                new UserCreated(
                    name.GetValue(),
                    identificationNumber.GetValue(),
                    email.GetValue()
                )
            );
        }
    }
}