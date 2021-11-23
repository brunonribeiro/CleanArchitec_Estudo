using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.MongoDb.Base;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace Infra.MongoDb
{
    public sealed class CompanyRepository : Repository<Company>, ICompanyRepositoryMongoDb
    {
        private readonly static string _collectionName = "Company";

        public CompanyRepository(IConfiguration configuration) : base(configuration, _collectionName)
        {
        }

        public Company QueryByCnpj(string cnpj)
        {
            return _collection.AsQueryable().FirstOrDefault(x => x.Cnpj == cnpj);
        }
    }
}