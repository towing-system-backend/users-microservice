using MongoDB.Driver;
using Application.Core;
using User.Application;

namespace User.Infrastructure
{
    public class FindUsersNameQuery : IService<string, List<FindUsersNameResponse>>
    {
        private readonly IMongoCollection<MongoUser> _userCollection;
        public FindUsersNameQuery()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<Result<List<FindUsersNameResponse>>> Execute(string param)
        {
            var filter = Builders<MongoUser>.Filter.Empty;
            var res = await _userCollection.Find(filter).ToListAsync();

            if (res == null) return Result<List<FindUsersNameResponse>>.MakeError(new UserNotFoundError());

            var users = res.Select(
                user => new FindUsersNameResponse(
                    user.UserId,
                    user.Name
                )
            ).ToList();

            return Result<List<FindUsersNameResponse>>.MakeSuccess(users);
        }
    }
}