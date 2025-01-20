namespace User.Infrastructure
{
    public record FindUserByIdResponse(
        string UserId,
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