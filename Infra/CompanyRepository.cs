using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Redis;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Infra.Redis
{
    public class CompanyRepository : ICompanyRepositoryRedis
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly RedisConfiguration _config;
        private readonly RedisClient _redisClient;

        public CompanyRepository(IConfiguration configuration, ILogger<CompanyRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _config = new RedisConfiguration
            {
                Host = _configuration.GetSection("Redis:Host").Value,
            };

            _redisClient = new RedisClient(_config.Host);
        }

        public Company Get(string cnpj)
        {
            var contentArray = _redisClient.Get(cnpj);
            if (contentArray != null)
            {
                var contentString = Encoding.UTF8.GetString(contentArray);
                var company = JsonConvert.DeserializeObject<Company>(contentString);

                _logger.LogInformation(GenerateLog(company, "Read Database Redis"));
                return company;
            }

            return null;
        }


        public void Save(Company company)
        {
            _redisClient.Set(company.Cnpj, company, new TimeSpan(0, 30, 0));
            _logger.LogInformation(GenerateLog(company, "Save Database Redis"));
        }

        public void Delete(Company company)
        {
            _redisClient.Remove(company.Cnpj);
            _logger.LogInformation(GenerateLog(company, "Delete Database Redis"));
        }

        private static string GenerateLog(Company company, string description)
        {
            return $"{description} ({DateTime.Now}): {company?.Id} - {company?.Name} - {company?.Cnpj} {(!string.IsNullOrWhiteSpace(company?.Email) ? "- " + company?.Email : "")} - {company?.FoundationDate.ToShortDateString()}";
        }
    }
}