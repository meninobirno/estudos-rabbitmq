using HelloWorld;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Estudos_RabbitMQ
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });
            builder.ConfigureServices((hostingService, services) =>
            {
                services.AddOptions();
                services.AddSingleton<IHostedService, HelloWorldProducer>();

            });
            await builder.RunConsoleAsync();
        }
    }
}
