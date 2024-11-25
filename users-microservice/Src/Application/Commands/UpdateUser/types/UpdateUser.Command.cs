namespace User.Application
{
    public record UpdateUserCommand(string Id, string? Name, string? Email, int? IdentificationNumber);
}