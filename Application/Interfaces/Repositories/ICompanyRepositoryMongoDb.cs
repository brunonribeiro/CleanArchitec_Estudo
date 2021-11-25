using Domain.Entities;
using System.Linq;

namespace Application.Interfaces.Repositories
{
    public interface ICompanyRepositoryMongoDb
    {
        IQueryable<Company> QueryAll();

        Company Query(int id);

        Company QueryByCnpj(string cnpj);

        void Insert(Company obj);

        void Update(Company obj);
    }
}