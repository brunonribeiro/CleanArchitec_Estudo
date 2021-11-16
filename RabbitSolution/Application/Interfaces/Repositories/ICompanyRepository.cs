using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Company GetById(int id);
        void Save(Company company);       
    }
}
