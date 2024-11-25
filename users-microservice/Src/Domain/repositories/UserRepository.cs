using Application.Core;

namespace User.Domain
{
    public interface IUserRepository
    {
        Task<Optional<User>> FindById(string userId);
        Task<Optional<User>> FindByEmail(string email);
        Task Save(User user);
        Task Remove(string userId);
    }
}