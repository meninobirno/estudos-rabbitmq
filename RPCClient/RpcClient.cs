using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RPCClient
{
    public class RpcClient
    {
        private readonly IConnection conn;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            conn = factory.CreateConnection();
            channel = conn.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);
            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            props.Persistent = true;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                    respQueue.Add(response);
            };
            channel.BasicConsume(consumer, replyQueueName, true);
        }

        public string Call(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "rpc-queue", props, messageBytes);
            return respQueue.Take();
        }

        public void Close()
        {
            conn.Close();
        }
        
    }
    class Rpc
    {
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            var n = args.Length > 0 ? args[0] : "30";

            Console.WriteLine($"requesting: fib({n})");
            var response = rpcClient.Call(n);
            Console.WriteLine($"got: {response}");
            rpcClient.Close();
        }
    }
}
