using Domain.Interfaces;
using Infra.MongoDb.Context;
using Infra.MongoDb.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Infra.MongoDb.Base
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        protected readonly ILogger _logger;
        private readonly ConnectionFactory _connectionFactory;

        protected Repository(IConfiguration configuration, ILogger<Repository<T>> logger, string collectionName)
        {
            _connectionFactory = new ConnectionFactory(configuration);
            _collection = _connectionFactory.GetDatabase().GetCollection<T>(collectionName);
            _logger = logger;
        }

        protected readonly IMongoCollection<T> _collection;

        public IQueryable<T> QueryAll()
        {
            var list = _collection.AsQueryable();
            _logger.LogInformation($"Read List Database MongoDB ({DateTime.Now})");
            return list;
        }

        public T Query(int id)
        {
            var obj = _collection.AsQueryable().FirstOrDefault(x => x.Id == id);
            _logger.LogInformation(GenerateLog(obj, "Read Database MongoDB"));
            return obj;
        }

        public void Insert(T obj)
        {
            _collection.InsertOne(obj);
            _logger.LogInformation(GenerateLog(obj, "Save Database MongoDB"));
        }

        public void Update(T obj)
        {
            _collection.ReplaceOne(x => x.Id == obj.Id, obj);
            _logger.LogInformation(GenerateLog(obj, "Update Database MongoDB"));
        }

        public void Delete(T obj)
        {
            _collection.DeleteOne(x => x.Id == obj.Id);
            _logger.LogInformation(GenerateLog(obj, "Delete Database MongoDB"));
        }

        protected static string GenerateLog(T obj, string description)
        {
            return $"{description} ({DateTime.Now}): {obj?.Id}";
        }
    }
}