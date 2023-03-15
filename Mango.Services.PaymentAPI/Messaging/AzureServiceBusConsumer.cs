using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using PaymentProcessor;

namespace Mango.Services.PaymentAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string _serviceBusConnectionString;
		private readonly string _orderPaymentProcessTopic;
		private readonly string _orderPaymentSubscription;
		private readonly string _orderUpdatePaymentResultTopic;

        private readonly IConfiguration _config;
		private ServiceBusProcessor _orderPaymentProcessor;
		private IMessageBus _messageBus;
		private IProcessPayment _processPayment;

        public AzureServiceBusConsumer(IConfiguration configuration, IMessageBus messageBus, IProcessPayment processPayment)
		{
			_config = configuration;
			_processPayment = processPayment;
			_serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString");
			_orderPaymentProcessTopic = _config.GetValue<string>("OrderPaymentProcessTopic");
			_orderPaymentSubscription = _config.GetValue<string>("OrderPaymentProcessSubscription");
			_orderUpdatePaymentResultTopic = _config.GetValue<string>("OrderUpdatePaymentResultTopic");
            _messageBus = messageBus;

			// Creating processor
			var client = new ServiceBusClient(_serviceBusConnectionString);
            _orderPaymentProcessor = client.CreateProcessor(_orderPaymentProcessTopic, _orderPaymentSubscription); 
        }

		public async Task Start()
		{
            _orderPaymentProcessor.ProcessMessageAsync += ProcessPayments;
            _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
			await _orderPaymentProcessor.StartProcessingAsync();
		}

        public async Task Stop()
        {
            await _orderPaymentProcessor.StopProcessingAsync();
			await _orderPaymentProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

        private async Task ProcessPayments(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);
			PaymentRequestMessage paymentRequestMessage = null;

			if(body is not null)
			{
				paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);
			}

			var result = _processPayment.PaymentProcessor();

			// Can be done using automapper too
			UpdatePaymentResultMessage updatedPaymentMessage = new()
			{
				Status = true,
				OrderId = paymentRequestMessage.OrderId,
				Email = paymentRequestMessage.Email
			};

			try
			{
				await _messageBus.PublishMessage(updatedPaymentMessage, _orderUpdatePaymentResultTopic);
				await args.CompleteMessageAsync(args.Message);
			}
			catch(Exception e)
			{
				throw;
			}
		}
	}
}

