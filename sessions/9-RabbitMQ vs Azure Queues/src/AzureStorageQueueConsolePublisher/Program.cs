using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace AzureStorageQueueConsolePublisher
{
    public static class Program
    {
        private static readonly string ConnectionString = "Replace this text with the connection string to your own Azure Storage Queue";

        private static readonly string StoregeQueueName = "demo-storage-queue";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Storage Queue Demo - Console Publisher");

            QueueClient queue = new QueueClient(ConnectionString, StoregeQueueName);

            Console.WriteLine("Enter a message:");
            var message = Console.ReadLine();

            await InsertMessageAsync(queue, message);
            Console.WriteLine($"Sent: {message}");
        }

        static async Task InsertMessageAsync(QueueClient theQueue, string newMessage)
        {
            if (null != await theQueue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("The queue was created.");
            }

            await theQueue.SendMessageAsync(newMessage);
        }
    }
}
