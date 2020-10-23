using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStoreQueueConsoleConsumer
{
    public static class Program
    {
        private static readonly string ConnectionString = "Replace this text with the connection string to your own Azure Storage Queue";

        private static readonly string StoregeQueueName = "demo-storage-queue";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Storage Queue Demo - Console Consumer");

            QueueClient queue = new QueueClient(ConnectionString, StoregeQueueName);

            string value = await RetrieveNextMessageAsync(queue);
            Console.WriteLine($"Received: {value}");

            Console.ReadLine();
        }

        private static async Task<string> RetrieveNextMessageAsync(QueueClient theQueue)
        {
            string theMessage = string.Empty;

            if (await theQueue.ExistsAsync())
            {
                QueueProperties properties = await theQueue.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await theQueue.ReceiveMessagesAsync(1);
                    theMessage = retrievedMessage[0].MessageText;

                    await theQueue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                }
            }

            return theMessage;
        }
    }
}
