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
                Domain.User.Create(new UserId(res.UserId),
                    new UserName(res.Name),
                    new UserIdentificationNumber(res.IdentificationNumber),
                    new UserEmail(res.Email),
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
                Domain.User.Create(new UserId(res.UserId),
                    new UserName(res.Name),
                    new UserIdentificationNumber(res.IdentificationNumber),
                    new UserEmail(res.Email),
                    true
                )
            );
        }

        public async Task Save(Domain.User user)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, user.GetUserId().GetValue());
            var update = Builders<MongoUser>.Update
                .Set(account => account.Name, user.GetUserName().GetValue())
                .Set(account => account.Email, user.GetUserEmail().GetValue())
                .Set(account => account.IdentificationNumber, user.GetUserIdentificationNumber().GetValue());

            await _userCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task Remove(string id)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, id);
            await _userCollection.DeleteOneAsync(filter);
        }
    }
}