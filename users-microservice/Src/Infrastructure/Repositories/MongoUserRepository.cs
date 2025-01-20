using IOptional = Application.Core.Optional<User.Domain.User>;
using MongoDB.Driver;
using User.Domain;

namespace User.Infrastructure
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<MongoUser> _userCollection;
        public MongoUserRepository()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<IOptional> FindById(string id)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, id);
            var res = await _userCollection.Find(filter).FirstOrDefaultAsync();

            if (res == null) return IOptional.Empty();

            return IOptional.Of(
                Domain.User.Create(
                    new UserId(res.UserId),
                    new SupplierCompanyId(res.SupplierCompanyId),
                    new UserName(res.Name),
                    new UserImage(res.Image),
                    new UserEmail(res.Email),
                    new UserRole(res.Role),
                    new UserStatus(res.Status),
                    new UserPhoneNumber(res.PhoneNumber),
                    new UserIdentificationNumber(res.IdentificationNumber),
                    true
                )
            );
        }

        public async Task<IOptional> FindByEmail(string email)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.Email, email);
            var res = await _userCollection.Find(filter).FirstOrDefaultAsync();

            if (res == null) return IOptional.Empty();

            return IOptional.Of(
                Domain.User.Create(
                    new UserId(res.UserId),
                    new SupplierCompanyId(res.SupplierCompanyId),
                    new UserName(res.Name),
                    new UserImage(res.Image),
                    new UserEmail(res.Email),
                    new UserRole(res.Role),
                    new UserStatus(res.Status),
                    new UserPhoneNumber(res.PhoneNumber),
                    new UserIdentificationNumber(res.IdentificationNumber),
                    true
                )
            );
        }

        public async Task Save(Domain.User user)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, user.GetUserId().GetValue());
            var update = Builders<MongoUser>.Update
                .Set(user => user.SupplierCompanyId, user.GetSupplierCompanyId().GetValue())
                .Set(user => user.Name, user.GetUserName().GetValue())
                .Set(user => user.Image, user.GetUserImage().GetValue())
                .Set(user => user.Email, user.GetUserEmail().GetValue())
                .Set(user => user.Role, user.GetUserRole().GetValue())
                .Set(user => user.Status, user.GetStatus().GetValue())
                .Set(user => user.PhoneNumber, user.GetUserPhoneNumber().GetValue())
                .Set(user => user.IdentificationNumber, user.GetUserIdentificationNumber().GetValue())
                .SetOnInsert(user => user.CreatedAt, DateTime.Now);
            
            await _userCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task Remove(string id)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, id);
            
            await _userCollection.DeleteOneAsync(filter);
        }
    }
}