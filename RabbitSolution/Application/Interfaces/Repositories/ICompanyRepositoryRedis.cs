using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICompanyRepositoryRedis
    {
        Company GetByCnpj(string cnpj);

        void Save(Company company);
    }
}