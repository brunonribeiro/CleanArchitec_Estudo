using Application.Core;
using Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateHandler : IRequestHandler<CompanyUpdateCommand, Response>
    {
        private readonly ICompanyRepositoryRedis _companyRepositoryRedis;
        private readonly ICompanyRepositoryMongoDb _companyRepositoryMongoDb;

        public CompanyUpdateHandler(ICompanyRepositoryRedis companyRepositoryRedis, ICompanyRepositoryMongoDb companyRepositoryMongoDb)
        {
            _companyRepositoryRedis = companyRepositoryRedis;
            _companyRepositoryMongoDb = companyRepositoryMongoDb;
        }

        public async Task<Response> Handle(CompanyUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var company = _companyRepositoryMongoDb.QueryByCnpj(request.Cnpj);

                if (company == null)
                    return new Response().AddError(Constants.MsgCompanyNotFound);

                company.UpdateName(request.Name);
                company.UpdateEmail(request.Email);
                company.UpdateFoundationDate(request.FoundationDate.ToDate());

                _companyRepositoryRedis.Save(company);
                _companyRepositoryMongoDb.Update(company);
            }
            catch
            {
                return new Response().AddError(Constants.MsgUnexpectedError);
            }

            return new Response(Constants.MsgCompanyChanged);
        }
    }
}