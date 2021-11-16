﻿using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Logging;
using System;
using Application.UseCases.CompanySaveUseCase;
using Application.DTO;

namespace RabbitMq
{
    public class RabbitService : IRabbitService
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _config;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IModel _chanel;
        private IConnection _connection;
        private EventingBasicConsumer _consumer;

        public RabbitService(IConfiguration configuration, ILogger<RabbitService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _config = new RabbitMqConfiguration
            {
                Host = _configuration.GetSection("RabbitMq:Host").Value,
                Queue = _configuration.GetSection("RabbitMq:Queue").Value
            };

            _factory = new ConnectionFactory
            {
                HostName = _config.Host
            };
        }

        public void Post(CompanySaveCommand company) 
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _config.Queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                     );

                    var messageSerialized = JsonConvert.SerializeObject(company);
                    var messageBytes = Encoding.UTF8.GetBytes(messageSerialized);

                    channel.BasicPublish(
                           exchange: "",
                           routingKey: _config.Queue,
                           basicProperties: null,
                           body: messageBytes
                        );

                    _logger.LogInformation(GenerateLog(company, "Publish RabbitMQ"));
                }
            }
        }

        public void Read()
        {
            _connection = _factory.CreateConnection();
            _chanel = _connection.CreateModel();

            _chanel.QueueDeclare(
                queue: _config.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
             );

            _consumer = new EventingBasicConsumer(_chanel);
            _consumer.Received += ConsumeMessage;

            _chanel.BasicConsume(_config.Queue, false, _consumer);
        }

        private void ConsumeMessage(object sender, BasicDeliverEventArgs eventArgs)
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);

            try
            {
                var company = JsonConvert.DeserializeObject<CompanySaveCommand>(contentString);

                _logger.LogInformation(GenerateLog(company, "Read RabbitMQ"));
            }
            catch
            {
                _logger.LogInformation("Read RabbitMQ: Dados incompativeis");
            }

            _chanel.BasicAck(eventArgs.DeliveryTag, false);
        }

        private static string GenerateLog(CompanySaveCommand company, string description)
        {
            return $"{description} ({DateTime.Now}): {company?.Name} - {company?.Cnpj} {(!string.IsNullOrWhiteSpace(company?.Email) ? "- " + company?.Email : "")} - {company?.FoundationDate}";
        }

    }
}