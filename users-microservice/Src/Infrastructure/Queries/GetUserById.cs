using IOptional = Application.Core.Optional<User.Infrastructure.GetUserByIdResponse>;
using MongoDB.Driver;

namespace User.Infrastructure
{
    public class GetUserById
    {
        private readonly IMongoCollection<MongoUser> _userCollection;
        public GetUserById()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<IOptional> Execute(string id)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, id);
            var res = await _userCollection.Find(filter).FirstOrDefaultAsync();

            if (res == null) return IOptional.Empty();

            return IOptional.Of(
                new GetUserByIdResponse(
                    res.UserId,
                    res.SupplierCompanyId,
                    res.Name,
                    res.Image,
                    res.Email,
                    res.Role,
                    res.Status,
                    res.PhoneNumber,
                    res.IdentificationNumber
                )
            );
        }
    }
}