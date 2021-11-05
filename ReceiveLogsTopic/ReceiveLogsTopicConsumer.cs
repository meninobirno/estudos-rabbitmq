using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ReceiveLogsTopic
{
    class ReceiveLogsTopicConsumer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare("topic-logs", ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("usage: {0} [binding_key...]",
                                            Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine("press any key to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queueName, "topic-logs", bindingKey);
                }

                Console.WriteLine("waiting for logs");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"message received: {ea.RoutingKey}:{message}");
                };
                channel.BasicConsume(queueName, true, consumer);

                Console.WriteLine("press any key to exit");
                Console.ReadLine();
            }
        }
    }
}
