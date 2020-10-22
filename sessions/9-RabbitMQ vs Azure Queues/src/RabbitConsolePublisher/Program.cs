using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitConsolePublisher
{
    public static class Program
    {
        private static readonly string DemoQueueName = "DemoQueue";

        private static readonly string DemoExchangeName = "RabbitDemo.exchange";

        private static readonly string DemoRoutingKey = "V1";

        static void Main(string[] args)
        {
            Console.WriteLine("RabbitMQ Demo - Console Publisher");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass,
                Port = AmqpTcpEndpoint.UseDefaultPort
            };

            IModel rabbitMqChannel;

            var rabbitMqConnection = factory.CreateConnection();
            rabbitMqChannel = rabbitMqConnection.CreateModel();

            //declare the queue  
            rabbitMqChannel.QueueDeclare(queue: DemoQueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            //bind queue to exchange
            rabbitMqChannel.QueueBind(DemoQueueName, DemoExchangeName, DemoRoutingKey);

            //publishing a message
            IBasicProperties properties = rabbitMqChannel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "text/plain";
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, DemoExchangeName, DemoRoutingKey);
            rabbitMqChannel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("This is a message from the RabbitMq .NET driver"));

            Console.ReadLine();
        }
    }
}
