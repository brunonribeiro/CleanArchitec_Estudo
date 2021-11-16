using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Company GetByCnpj(string cnpj);
        void Save(Company company);       
    }
}
