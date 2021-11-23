using Application.Interfaces.UseCases;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Consumer
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly ICompanyReceiverUseCase _companyReceiverUseCase;

        public RabbitConsumer(ICompanyReceiverUseCase companyReceiverUseCase)
        {
            _companyReceiverUseCase = companyReceiverUseCase;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _companyReceiverUseCase.Execute();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }
        }
    }
}