using Application.Core;
using MongoDB.Driver;
using User.Application;

namespace User.Infrastructure;

public class FindUsersByRoleQuery
{
    private readonly IMongoCollection<MongoUser> _userCollection;

    public FindUsersByRoleQuery()
    {
        MongoClient client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI_READ_MODELS"));
        IMongoDatabase database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME_READ_MODELS"));
        _userCollection = database.GetCollection<MongoUser>("users");
    }
    
    public async Task<Result<List<FindUsersByRoleResponse>>> Execute(string param)
    {
        var filter = Builders<MongoUser>.Filter.Eq(user => user.Role, param);
        var res = await _userCollection.Find(filter).ToListAsync();

        if (res == null) return Result<List<FindUsersByRoleResponse>>.MakeError(new UserNotFoundError());

        var users = res.Select(
            user => new FindUsersByRoleResponse(
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

        return Result<List<FindUsersByRoleResponse>>.MakeSuccess(users);
    }
}
