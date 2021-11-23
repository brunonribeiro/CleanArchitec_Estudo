using Infra.MongoDb.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infra.MongoDb.Context
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly MongoConfiguration _config;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;

            _config = new MongoConfiguration
            {
                ConnectionString = _configuration.GetSection("MongoDb:ConnectionString").Value,
                DatabaseName = _configuration.GetSection("MongoDb:DatabaseName").Value
            };
        }

        private IMongoClient GetClient()
        {
            return new MongoClient(_config.ConnectionString);
        }

        public IMongoDatabase GetDatabase()
        {
            var mongoClient = GetClient();
            return mongoClient.GetDatabase(_config.DatabaseName);
        }
    }
}