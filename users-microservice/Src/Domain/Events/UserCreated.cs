using Application.Core;

namespace User.Domain
{
    public class UserCreatedEvent(string publisherId, string type, UserCreated context) : DomainEvent(publisherId, type, context) { }

    public class UserCreated(
        string supplierCompanyId,
        string name,
        string image, 
        string email,
        string role,
        string status,
        string phoneNumber,
        int identificationNumber)
    {
        public readonly string SupplierCompanyId = supplierCompanyId;
        public readonly string Name = name;
        public readonly string Image = image;
        public readonly string Email = email;
        public readonly string Role = role;
        public readonly string Status = status;
        public readonly string PhoneNumber = phoneNumber;
        public readonly int IdentificationNumber = identificationNumber;

        public static UserCreatedEvent CreateEvent(
            UserId publisherId,
            SupplierCompanyId supplierCompanyId,
            UserName name, 
            UserImage image,
            UserEmail email,
            UserRole role,
            UserStatus status,
            UserPhoneNumber phoneNumber,
            UserIdentificationNumber identificationNumber)
        {
            return new UserCreatedEvent(
                publisherId.GetValue(),
                typeof(UserCreated).Name,
                new UserCreated(
                    supplierCompanyId.GetValue(),
                    name.GetValue(),
                    image.GetValue(),
                    email.GetValue(),
                    role.GetValue(),
                    status.GetValue(),
                    phoneNumber.GetValue(),
                    identificationNumber.GetValue()
                )
            );
        }
    }
}