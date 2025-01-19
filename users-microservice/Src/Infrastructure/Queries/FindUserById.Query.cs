using MongoDB.Driver;
using Application.Core;
using User.Application;

namespace User.Infrastructure
{
    public class FindUserByIdQuery : IService<string, FindUserByIdResponse>
    {
        private readonly IMongoCollection<MongoUser> _userCollection;
        public FindUserByIdQuery()
        {
            MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI"));
            IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            _userCollection = database.GetCollection<MongoUser>("users");
        }

        public async Task<Result<FindUserByIdResponse>> Execute(string id)
        {
            var filter = Builders<MongoUser>.Filter.Eq(user => user.UserId, id);
            var res = await _userCollection.Find(filter).FirstOrDefaultAsync();

            if (res == null) return Result<FindUserByIdResponse>.MakeError(new UserNotFoundError());

            return Result<FindUserByIdResponse>.MakeSuccess(
                new FindUserByIdResponse(
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