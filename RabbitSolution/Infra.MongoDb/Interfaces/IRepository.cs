using System;
using System.Linq;

namespace Infra.MongoDb.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> QueryAll();
        T Query(int id);
        void Insert(T obj);
        void Update(T obj);
    }
}