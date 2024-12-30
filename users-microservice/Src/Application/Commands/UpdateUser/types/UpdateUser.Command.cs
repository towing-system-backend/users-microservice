namespace User.Application
{
    public record UpdateUserCommand(
        string Id,
        string? SupplierCompanyId,
        string? Name,
        string? Image,
        string? Email,
        string? Role,
        string? Status,
        string? PhoneNumber,
        int? IdentificationNumber
    );
}