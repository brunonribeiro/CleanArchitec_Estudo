using MongoDB.Driver;

namespace Infra.MongoDb.Interfaces
{
    public interface IConnectionFactory
    {
        IMongoDatabase GetDatabase();
    }
}