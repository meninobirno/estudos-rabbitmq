using RabbitMQ.Client;
using System;
using System.Text;

namespace EmitLog
{
    class EmitLogProducer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare("logs", ExchangeType.Fanout);

                var message = GetMessage(args);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs", "", null, body);
                Console.WriteLine($"message published: {message}");
            }
            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "info: hello world!";
        }
    }
}
