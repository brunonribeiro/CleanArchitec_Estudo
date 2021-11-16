using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ILogger _logger;

        public CompanyRepository(ILogger<CompanyRepository> logger)
        {
            _logger = logger;
        }

        public List<Company> GetList()
        {
            return new List<Company>
            {
                new Company(1, "Empresa XX1", "40968837000143","", DateTime.Today.AddDays(-3000)),
                new Company(2, "Empresa XX2", "24886356000132","", DateTime.Today.AddDays(-2500)),
                new Company(3, "Empresa XX3", "84683917000128","", DateTime.Today.AddDays(-2000)),
                new Company(4, "Empresa XX4", "12285601000177","", DateTime.Today.AddDays(-1500)),
                new Company(5, "Empresa XX5", "65780064000106","", DateTime.Today.AddDays(-1000)),
                new Company(6, "Empresa XX6", "52847652000160","", DateTime.Today.AddDays(-0500))
            };
        }

        public Company GetById(int id)
        {
            var company = GetList().FirstOrDefault(x => x.Id == id);
            _logger.LogInformation(GenerateLog(company, "Read Database"));

            return company;
        }

        public void Save(Company company)
        {
            if (company.Id == 0)
            {
                var lastId = GetList().LastOrDefault()?.Id ?? 0;
                company.AssignId(++lastId);
            }

            _logger.LogInformation(GenerateLog(company, "Save Database"));
        }

        private static string GenerateLog(Company company, string description)
        {
            return $"{description} ({DateTime.Now}): {company?.Id} - {company?.Name} - {company?.Cnpj} {(!string.IsNullOrWhiteSpace(company?.Email) ? "- " + company?.Email : "")} - {company?.FoundationDate.ToShortDateString()}";
        }
    }
}
