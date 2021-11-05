using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Worker
{
    class WorkerConsumer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare("task-queue", true, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Message received: {message}");

                        //fake task to simulate execution time
                        var dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine($"{message} done.");

                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume("task-queue", false, consumer);

                Console.WriteLine("Press any key to end");
                Console.ReadLine();
            }
        }

    }
}
