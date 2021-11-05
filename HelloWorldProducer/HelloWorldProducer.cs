using RabbitMQ.Client;
using System;
using System.Text;

namespace HelloWorldProducer
{
    class HelloWorldProducer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    var message = "hello world";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello-world", null, body);

                    Console.WriteLine($"Message published: {message}");

                }
            }
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }
    }
}
