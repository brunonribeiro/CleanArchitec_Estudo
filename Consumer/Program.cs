using Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class Program
    {
        public static IConfiguration Configuration;
        public static ILogger<KafkaService> Logger;

        public static void Main(string[] args)
        {
            ConfigureServices(args);

            var kafkaService = new KafkaService(Configuration, Logger);

            kafkaService.Consume();
        }

        private static void ConfigureServices(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .AddCommandLine(args);

            Configuration = builder.Build();

            using ILoggerFactory loggerFactory =
            LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                }));

            Logger  = loggerFactory.CreateLogger<KafkaService>();
        }
    }
}
