using Application.Core;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateHandler : IRequestHandler<CompanyUpdateCommand, Response>
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyUpdateHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<Response> Handle(CompanyUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var company = _companyRepository.GetById(request.Id);

                if (company == null)
                    return new Response().AddError("Empresa não encontrada");

                company.UpdateName(request.Name);
                company.UpdateEmail(request.Email);
                company.UpdateFoundationDate(request.FoundationDate.ToDate());

                _companyRepository.Save(company);
            }
            catch
            {
                throw;
            }

            return new Response("Empresa alerada com sucesso");
        }
    }
}
