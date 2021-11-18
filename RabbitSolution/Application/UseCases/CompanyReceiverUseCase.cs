using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using System;

namespace Application.UseCases
{
    public class CompanyReceiverUseCase : ICompanyReceiverUseCase
    {
        private readonly IRabbitService _rabbitService;

        public CompanyReceiverUseCase(IRabbitService rabbitService)
        {
            _rabbitService = rabbitService;
        }

        public void Execute()
        {
            try
            {
                _rabbitService.Read();
            }
            catch
            {
                throw new ArgumentNullException();
            }
        }
    }
}
