using Application.Core;
using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CompanyDeleteUseCase
{
    public class CompanyDeleteHandler : IRequestHandler<CompanyDeleteCommand, Response>
    {
        private readonly ICompanyRepositoryRedis _companyRepositoryRedis;
        private readonly ICompanyRepositoryMongoDb _companyRepositoryMongoDb;

        public CompanyDeleteHandler(ICompanyRepositoryRedis companyRepositoryRedis, ICompanyRepositoryMongoDb companyRepositoryMongoDb)
        {
            _companyRepositoryRedis = companyRepositoryRedis;
            _companyRepositoryMongoDb = companyRepositoryMongoDb;
        }

        public async Task<Response> Handle(CompanyDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                DeleteCache(request);
                return DeleteDatabase(request);
            }
            catch
            {
                return new Response().AddError(Constants.MsgUnexpectedError);
            }
        }

        private void DeleteCache(CompanyDeleteCommand request)
        {
            var companySaved = _companyRepositoryRedis.Get(request.Cnpj);

            if (companySaved == null)
                return;

            _companyRepositoryRedis.Delete(companySaved);
        }

        private Response DeleteDatabase(CompanyDeleteCommand request)
        {
            var companySaved = _companyRepositoryMongoDb.QueryByCnpj(request.Cnpj);

            if (companySaved == null)
                return new Response().AddError(Constants.MsgCompanyNotFound);

            _companyRepositoryMongoDb.Delete(companySaved);
            return new Response(Constants.MsgCompanyDeleted);
        }
    }
}
