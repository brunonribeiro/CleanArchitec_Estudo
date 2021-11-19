using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.CompanySaveUseCase
{
    public class CompanySaveHandler : IRequestHandler<CompanySaveCommand, Response>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IRabbitService _rabbitService;

        public CompanySaveHandler(ICompanyRepository companyRepository, IRabbitService rabbitService)
        {
            _companyRepository = companyRepository;
            _rabbitService = rabbitService;
        }

        public async Task<Response> Handle(CompanySaveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Envia mensagem para RabbitMQ
                _rabbitService.Post(request);

                var company = new Company(
                   request.Name,
                   request.Cnpj,
                   request.Email,
                   request.FoundationDate.ToDate().Value
                );

                //Salva no banco de dados
                _companyRepository.Save(company);
            }
            catch
            {
                return new Response().AddError(Constants.MsgUnexpectedError);
            }

            return new Response(Constants.MsgCompanyRegistered);
        }
    }
}
