using User.Infrastructure;

namespace RabbitMQ.Contracts
{
    public class CreateUserDtoCreator : DtoCreator<CreateUser, CreateUserDto>
    {
        public override CreateUserDto CreateDto(CreateUser message)
        {
            return new CreateUserDto(
                message.Id,
                message.SupplierCompanyId,
                message.Name,
                message.Image,
                message.Email,
                message.Role,
                message.Status,
                message.PhoneNumber,
                message.IdentificationNumber
            );
        }
    }
}