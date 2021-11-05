using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace EmitLogTopic
{
    class EmitLogTopicProducer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare("topic-logs", ExchangeType.Topic);

                var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";

                var message = GetMessage(args);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("topic-logs", routingKey, null, body);
                Console.WriteLine($"message published: {routingKey}:{message}");
            }
            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args.Skip(1).ToArray()) : "hello world!";
        }
    }
}
