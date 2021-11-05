using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class HelloWorldProducer : IHostedService
    {
        private readonly IHostApplicationLifetime _lifetime;

        public HelloWorldProducer(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("hello-world", false, false, false, null);
                    var message = "hello world";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello-world", null, body);

                    Console.WriteLine($"Message publicada: {message}");

                }
            }
            Console.WriteLine("Aperte qualquer tecla para encerrar.");
            Console.ReadKey();
            _lifetime.StopApplication();

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Aplicação encerrada.");
            _lifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}
