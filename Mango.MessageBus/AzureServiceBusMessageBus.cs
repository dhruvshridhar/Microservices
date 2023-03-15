using System;
using Azure.Core;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
	public class AzureServiceBusMessageBus : IMessageBus
	{
        // Can be read from appsettings
        private readonly string connectionString = "Endpoint=sb://mangorestraunt.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=c25CluvuBOoGIkFzsyMQtC0n0FpfVxRYS+KFd8hr01U=";
		public AzureServiceBusMessageBus()
		{
		}

        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(jsonMessage)
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            // send the message
            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}

