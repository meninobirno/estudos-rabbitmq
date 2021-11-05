using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace HelloWorldConsumer
{
    class HelloWorldConsumer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Message received: {message}");
                    };
                    channel.BasicConsume("hello-world", true, consumer);
                }
            }
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }
    }
}
