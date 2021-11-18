﻿using Application.Core;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System;
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
                var company = _companyRepository.GetByCnpj(request.Cnpj);

                if (company == null)
                    return new Response().AddError(Constantes.EmpresaNaoEncontrada);

                company.UpdateName(request.Name);
                company.UpdateEmail(request.Email);
                company.UpdateFoundationDate(request.FoundationDate.ToDate());

                _companyRepository.Save(company);
            }
            catch
            {
                throw new ArgumentNullException();
            }

            return new Response(Constantes.EmpresaAlterada);
        }
    }
}
