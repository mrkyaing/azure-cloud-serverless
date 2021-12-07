using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace ServiceBusSenderApp
{
    public class Program
    {
        // connection string to your Service Bus namespace
        static string connectionString = "Endpoint=sb://allsharedservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=L4G7U0Q/tjbMCUH4R89cr2qVaGn7LjqKn+vzAlNG/rQ=";

        // name of your Service Bus topic
        static string topicName = "mytopic";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the sender used to publish messages to the topic
        static ServiceBusSender sender;

        // number of messages to be sent to the topic
        private const int numOfMessages = 3;
        static async Task Main(string[] args)
        {
            await SendMessageToTopic();
        }

        static async Task SendMessageToTopic()
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(topicName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the topic.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
