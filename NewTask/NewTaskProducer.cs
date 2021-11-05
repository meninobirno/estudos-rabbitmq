using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class NewTaskProducer
    {
        static void Main(string[] args)
        {
            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare("task-queue", true, false, false);

                    var prop = channel.CreateBasicProperties();
                    prop.Persistent = true;

                    channel.BasicPublish("", "task-queue", prop, body);

                    Console.WriteLine($"{message} published");
                }
            }
            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
        }
    }
}
