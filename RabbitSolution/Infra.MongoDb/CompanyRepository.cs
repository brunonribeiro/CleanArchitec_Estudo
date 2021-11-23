using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.MongoDb.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq;

namespace Infra.MongoDb
{
    public sealed class CompanyRepository : Repository<Company>, ICompanyRepositoryMongoDb
    {
        private readonly static string _collectionName = "Company";

        public CompanyRepository(IConfiguration configuration, ILogger<CompanyRepository> logger) : base(configuration, logger, _collectionName)
        {
        }

        public Company QueryByCnpj(string cnpj)
        {
            var company = _collection.AsQueryable().FirstOrDefault(x => x.Cnpj == cnpj);
            _logger.LogInformation(GenerateLog(company, "Read Database MongoDB"));
            return company;
        }
    }
}