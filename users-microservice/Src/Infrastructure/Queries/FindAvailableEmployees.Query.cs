using Application.Core;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using User.Application;
using Users.Infrastructure;

namespace User.Infrastructure
{
    public class FindAvailableEmployeesQuery
    {
        private readonly IMongoCollection<MongoUser> _userCollection;

        public FindAvailableEmployeesQuery()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI_READ_MODELS"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME_READ_MODELS"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<Result<List<FindAvailableEmployeesResponse>>> Execute()
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.Role, "Employee");
            var res = await _userCollection.Find(filter).ToListAsync();

            if (res.IsNullOrEmpty()) return Result<List<FindAvailableEmployeesResponse>>.MakeError(new UserNotFoundError());

            var users = res.Select(
                user => new FindAvailableEmployeesResponse(
                    user.UserId,
                    user.Name
                )
            ).ToList();

            return Result<List<FindAvailableEmployeesResponse>>.MakeSuccess(users);
        }
    }
}
