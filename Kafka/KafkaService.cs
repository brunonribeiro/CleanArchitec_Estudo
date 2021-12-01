using Application.DTO;
using Application.Interfaces.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;

namespace Kafka
{
    public class KafkaService : IKafkaService
    {
        private readonly string _host;
        private readonly string _kafkaTopic;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public KafkaService(IConfiguration configuration, ILogger<KafkaService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _host = _configuration.GetSection("Kafka:Host").Value;
            _kafkaTopic = _configuration.GetSection("Kafka:Topic").Value;
        }

        public async void Produce(CompanyDto company)
        {
            var config = new ProducerConfig { BootstrapServers = _host };

            using (var producerBuilder = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var messageSerialized = JsonConvert.SerializeObject(company);

                    var dr = await producerBuilder.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = messageSerialized });

                    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                    _logger.LogInformation(GenerateLog(company, "Publish Kafka"));
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                    _logger.LogError(GenerateLog(company, "Error Publish Kafka"));
                }
            }
        }

        public void Consume()
        {
            Console.WriteLine("Waiting...");

            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = _host,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            {
                consumer.Subscribe(_kafkaTopic);

                var Cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    Cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var message = consumer.Consume(Cts.Token);
                            _logger.LogInformation($"Received message '{message.Message.Value}' from: '{message.TopicPartitionOffset}'");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Error occured:");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        private static string GenerateLog(CompanyDto company, string description)
        {
            return $"{description} ({DateTime.Now}): {company?.Name} - {company?.Cnpj} {(!string.IsNullOrWhiteSpace(company?.Email) ? "- " + company?.Email : "")} - {company?.FoundationDate}";
        }
    }
}
