using Application.Core;

namespace User.Domain
{
    public class User : AggregateRoot<UserId>
    {
        private new UserId Id;
        private UserName Name;
        private UserEmail Email;
        private UserIdentificationNumber IdentificationNumber;

        private User(UserId userId) : base(userId)
        {
            Id = userId;
        }

        public override void ValidateState()
        {
            if (Id == null ||
                Name == null ||
                Email == null ||
                IdentificationNumber == null
            )
                throw new InvalidUserException();
        }

        public UserId GetUserId() => Id;
        public UserName GetUserName() => Name;
        public UserEmail GetUserEmail() => Email;
        public UserIdentificationNumber GetUserIdentificationNumber() => IdentificationNumber;

        public static User Create(UserId userId, UserName name, UserIdentificationNumber identificationNumber, UserEmail email, bool fromPersistence = false)
        {
            if (fromPersistence)
            {
                return new User(userId)
                {
                    Id = userId,
                    Name = name,
                    IdentificationNumber = identificationNumber,
                    Email = email
                };
            }
            var user = new User(userId);
            user.Apply(UserCreated.CreateEvent(userId, name, identificationNumber, email));

            return user;
        }

        public void UpdateUserName(UserName name)
        {
            Apply(UserNameUpdated.CreateEvent(Id, name));
        }

        public void UpdateUserEmail(UserEmail email)
        {
            Apply(UserEmailUpdated.CreateEvent(Id, email));
        }

        public void UpdateUserIdentificationNumber(UserIdentificationNumber identificationNumber)
        {
            Apply(UserIdentificationNumberUpdated.CreateEvent(Id, identificationNumber));
        }

        private void OnUserCreatedEvent(UserCreated context)
        {
            Name = new UserName(context.Name);
            IdentificationNumber = new UserIdentificationNumber(context.IdentificationNumber);
            Email = new UserEmail(context.Email);
        }

        private void OnUserNameUpdatedEvent(UserNameUpdated context)
        {
            Name = new UserName(context.Name);
        }

        private void OnUserEmailUpdatedEvent(UserEmailUpdated context)
        {
            Email = new UserEmail(context.Email);
        }
        private void OnUserIdentificationNumberUpdatedEvent(UserIdentificationNumberUpdated context)
        {
            IdentificationNumber = new UserIdentificationNumber(context.IdentificationNumber);
        }
    }
}