using Application.Core;

namespace User.Domain
{
    public class User : AggregateRoot<UserId>
    {
        private new UserId _id;
        private SupplierCompanyId _supplierCompanyId;
        private UserName _name;
        private UserImage _image;
        private UserEmail _email;
        private UserRole _role;
        private UserStatus _status;
        private UserPhoneNumber _phoneNumber;
        private UserIdentificationNumber _identificationNumber;

        private User(UserId userId) : base(userId)
        {
            _id = userId;
        }

        public override void ValidateState()
        {
            if (Id == null ||
                _supplierCompanyId == null ||
                _name == null ||
                _image == null ||
                _email == null ||
                _role == null ||
                _status == null ||
                _phoneNumber == null ||
                _identificationNumber == null
            )
                throw new InvalidUserException();
        }

        public UserId GetUserId() => _id;

        public SupplierCompanyId GetSupplierCompanyId() => _supplierCompanyId;

        public UserName GetUserName() => _name;

        public UserImage GetUserImage() => _image;

        public UserEmail GetUserEmail() => _email;

        public UserRole GetUserRole() => _role;

        public UserStatus GetStatus() => _status;

        public UserPhoneNumber GetUserPhoneNumber() => _phoneNumber;

        public UserIdentificationNumber GetUserIdentificationNumber() => _identificationNumber;

        public static User Create(
            UserId userId,
            SupplierCompanyId supplierCompanyId,
            UserName name,
            UserImage image,
            UserEmail email,
            UserRole role,
            UserStatus status,
            UserPhoneNumber phoneNumber,
            UserIdentificationNumber identificationNumber,
            bool fromPersistence = false)
        {
            if (fromPersistence)
            {
                return new User(userId)
                {
                    _id = userId,
                    _supplierCompanyId = supplierCompanyId,
                    _name = name,
                    _image = image,
                    _email = email,
                    _role = role,
                    _status = status,
                    _phoneNumber = phoneNumber,
                    _identificationNumber = identificationNumber,
                };
            }
            var user = new User(userId);
            user.Apply(UserCreated.CreateEvent(
                    userId,
                    supplierCompanyId,
                    name,
                    image,
                    email,
                    role,
                    status,
                    phoneNumber,
                    identificationNumber
                )
            );

            return user;
        }

        public void UpdateSupplierCompanyId(SupplierCompanyId id)
        {
            Apply(SupplierCompanyIdUpdated.CreateEvent(Id, id));
        }
        
        public void UpdateUserName(UserName name)
        {
            Apply(UserNameUpdated.CreateEvent(Id, name));
        }

        public void UpdateUserImage(UserImage image)
        {
            Apply(UserImageUpdated.CreateEvent(Id, image));
        }

        public void UpdateUserEmail(UserEmail email)
        {
            Apply(UserEmailUpdated.CreateEvent(Id, email));
        }

        public void UpdateUserRole(UserRole role)
        {
            Apply(UserRoleUpdated.CreateEvent(Id, role));
        }

        public void UpdateUserStatus(UserStatus status)
        {
            Apply(UserStatusUpdated.CreateEvent(Id, status));
        }

        public void UpdateUserPhoneNumber(UserPhoneNumber phoneNumber)
        {
            Apply(UserPhoneNumberUpdated.CreateEvent(Id, phoneNumber));
        }

        public void UpdateUserIdentificationNumber(UserIdentificationNumber identificationNumber)
        {
            Apply(UserIdentificationNumberUpdated.CreateEvent(Id, identificationNumber));
        }

        private void OnUserCreatedEvent(UserCreated context)
        {
            _supplierCompanyId = new SupplierCompanyId(context.SupplierCompanyId);
            _name = new UserName(context.Name);
            _image = new UserImage(context.Image);
            _email = new UserEmail(context.Email);
            _role = new UserRole(context.Role);
            _status = new UserStatus(context.Status);
            _phoneNumber = new UserPhoneNumber(context.PhoneNumber);
            _identificationNumber = new UserIdentificationNumber(context.IdentificationNumber);
        }

        private void OnSupplierCompanyIdUpdatedEvent(SupplierCompanyIdUpdated context)
        {
            _supplierCompanyId = new SupplierCompanyId(context.SupplierCompanyId);
        }

        private void OnUserNameUpdatedEvent(UserNameUpdated context)
        {
            _name = new UserName(context.Name);
        }

        private void OnUserImageUpdatedEvent(UserImageUpdated context)
        {
            _image = new UserImage(context.Image);
        }

        private void OnUserEmailUpdatedEvent(UserEmailUpdated context)
        {
            _email = new UserEmail(context.Email);
        }

        private void OnUserRoleUpdatedEvent(UserRoleUpdated context)
        {
            _role = new UserRole(context.Role);
        }

        private void OnUserStatusUpdatedEvent(UserStatusUpdated context)
        {
            _status = new UserStatus(context.Status);
        }

        private void OnUserPhoneNumberUpdatedEvent(UserPhoneNumberUpdated context)
        {
            _phoneNumber = new UserPhoneNumber(context.PhoneNumber);
        }

        private void OnUserIdentificationNumberUpdatedEvent(UserIdentificationNumberUpdated context)
        {
            _identificationNumber = new UserIdentificationNumber(context.IdentificationNumber);
        }
    }
}