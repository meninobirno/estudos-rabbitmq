using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RPCServer
{
    class RPCServer
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare("rpc-queue", true, false, false);
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume("rpc-queue", false, consumer);
                Console.WriteLine("awaiting rpc requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;
                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;
                    replyProps.Persistent = true;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        var n = int.Parse(message);
                        Console.WriteLine($"message receveid: {message}");
                        response = fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                };
                Console.WriteLine("press any key to exit");
                Console.ReadLine();
            }
        }
        private static int fib(int n)
        {
            if (n == 0 || n == 1)
                return n;
            return fib(n - 1) + fib(n - 2);
        }
    }
}
