namespace User.Application
{
    public record RegisterUserCommand(string Name, string Email, int IdentificationNumber);
}