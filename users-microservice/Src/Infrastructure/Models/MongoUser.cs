using MongoDB.Bson.Serialization.Attributes;

namespace User.Infrastructure
{
    public class MongoUser(string userId, string name, string email, int identificationNumber)
    {
        [BsonId]
        public string UserId = userId;
        public string Name = name;
        public string Email = email;
        public int IdentificationNumber = identificationNumber;
    }
}

