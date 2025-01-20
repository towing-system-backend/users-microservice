namespace User.Application
{
    public record RegisterUserCommand(
        string Id,
        string SupplierCompanyId,
        string Name,
        string Image,
        string Email,
        string Role,
        string Status,
        string PhoneNumber,
        int IdentificationNumber
    );
}