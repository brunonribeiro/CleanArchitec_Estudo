using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases;
using FluentValidation;
using Infra;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq;
using System;

namespace IoC
{
    public static class Bootstrapper
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddSingleton<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<ICompanyReceiverUseCase, CompanyReceiverUseCase>();
            services.AddSingleton<IRabbitService, RabbitService>();

            AddMediatr(services);
        }

        private static void AddMediatr(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Application");

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));

            services.AddMediatR(assembly);
        }
    }
}
