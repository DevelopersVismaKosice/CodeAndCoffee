using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitConsoleConsumer
{
    public static class Program
    {
        private const string CONFIG_QUEUE = "DemoQueue";

        private static IModel rabbitMqChannel;

        static void Main(string[] args)
        {
            Console.WriteLine("RabbitMQ Demo - Console Consumer");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass,
                Port = AmqpTcpEndpoint.UseDefaultPort
            };

            var rabbitMqConnection = factory.CreateConnection();
            rabbitMqChannel = rabbitMqConnection.CreateModel();

            //declare the queue  
            rabbitMqChannel.QueueDeclare(queue: CONFIG_QUEUE,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            //consume the message received  
            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            consumer.Received += MessageReceived;

            rabbitMqChannel.BasicConsume(queue: CONFIG_QUEUE,
                                         autoAck: false,
                                         consumer: consumer);
            Console.ReadLine();

        }

        private static void MessageReceived(object sender, BasicDeliverEventArgs args)
        {
            IBasicProperties basicProperties = args.BasicProperties;
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());

            Console.WriteLine($"Message received from the exchange {args.Exchange}");
            Console.WriteLine($"Content type: {basicProperties.ContentType}");
            Console.WriteLine($"Consumer tag: {args.ConsumerTag}");
            Console.WriteLine($"Delivery tag: {args.DeliveryTag}");
            Console.WriteLine($"Message: {message}");

            rabbitMqChannel.BasicAck(args.DeliveryTag, false);
        }

    }
}
