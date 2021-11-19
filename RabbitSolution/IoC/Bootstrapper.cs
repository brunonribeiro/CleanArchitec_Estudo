using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases;
using FluentValidation;
using Infra;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq;
using System;

namespace IoC
{
    public static class Bootstrapper
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<ICompanyReceiverUseCase, CompanyReceiverUseCase>();
            services.AddSingleton<IRabbitService, RabbitService>();
            services.AddHealthCheck(configuration);
            services.AddMediatr();
        }

        private static void AddMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Application");

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));

            services.AddMediatR(assembly);
        }

        private static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthCheckRabbitMQ(configuration);

            services.AddHealthChecks()
                .AddRedis(configuration.GetSection("Redis:Host").Value,
                    name: "redis", tags: new string[] { "redis", "infra" });

            services.AddHealthChecksUI()
                .AddInMemoryStorage();
        }

        private static void AddHealthCheckRabbitMQ(this IServiceCollection services,IConfiguration configuration)
        {
            var config = new RabbitMqConfiguration
            {
                Host = configuration.GetSection("RabbitMq:Host").Value,
                Port = configuration.GetSection("RabbitMq:Port").Value,
                User = configuration.GetSection("RabbitMq:User").Value,
                Password = configuration.GetSection("RabbitMq:Password").Value
            };

            var connectionString = $"amqp://{config.User}:{config.Password}@{config.Host}:{config.Port}/";

            services.AddHealthChecks()
                .AddRabbitMQ(connectionString,
                    name: "rabbitmq", tags: new string[] { "rabbit", "mensageria" });
        }
    }
}
