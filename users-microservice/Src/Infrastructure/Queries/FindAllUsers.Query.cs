using Application.Core;
using MongoDB.Driver;
using User.Application;

namespace User.Infrastructure
{
    public class FindAllUsersQuery 
    {
        private readonly IMongoCollection<MongoUser> _userCollection;

        public FindAllUsersQuery()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI_READ_MODELS"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME_READ_MODELS"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<Result<List<FindAllUsersResponse>>> Execute(string param)
        {
            var filter = Builders<MongoUser>.Filter.Empty;
            var res = await _userCollection.Find(filter).ToListAsync();

            if (res == null) return Result<List<FindAllUsersResponse>>.MakeError(new UserNotFoundError());

            var users = res.Select(
                user => new FindAllUsersResponse(
                    user.UserId,
                    user.SupplierCompanyId,
                    user.Name,
                    user.Image,
                    user.Email,
                    user.Role,
                    user.Status,
                    user.PhoneNumber,
                    user.IdentificationNumber
                )
            ).ToList();

            return Result<List<FindAllUsersResponse>>.MakeSuccess(users);
        }
    }
}