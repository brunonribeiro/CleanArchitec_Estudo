using Domain.Interfaces;
using Infra.MongoDb.Context;
using Infra.MongoDb.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace Infra.MongoDb.Base
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        private readonly ConnectionFactory _connectionFactory;

        protected Repository(IConfiguration configuration, string collectionName)
        {
            _connectionFactory = new ConnectionFactory(configuration);
            _collection = _connectionFactory.GetDatabase().GetCollection<T>(collectionName);
        }

        protected readonly IMongoCollection<T> _collection;

        public IQueryable<T> QueryAll()
        {
            return _collection.AsQueryable();
        }

        public T Query(int id)
        {
            return _collection.AsQueryable().FirstOrDefault(x => x.Id == id);
        }

        public void Insert(T obj)
        {
            _collection.InsertOne(obj);
        }

        public void Update(T obj)
        {
            _collection.ReplaceOne(x => x.Id == obj.Id, obj);
        }
    }
}