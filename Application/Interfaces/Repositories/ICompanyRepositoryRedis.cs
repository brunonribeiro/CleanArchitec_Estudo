using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICompanyRepositoryRedis
    {
        Company Get(string cnpj);
        void Save(Company company);
        void Delete(Company company);
    }
}