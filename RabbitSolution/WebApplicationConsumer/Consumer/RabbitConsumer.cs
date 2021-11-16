﻿using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.UseCases;

namespace WebApplicationConsumer.Consumer
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
