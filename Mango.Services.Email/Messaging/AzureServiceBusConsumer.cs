using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;

namespace Mango.Services.Email.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly EmailRepository _emailRepository;
		private readonly string _serviceBusConnectionString;
        private readonly string _subscriptionEmail;
		private readonly string _orderUpdatePaymentResultTopic;

        private readonly IConfiguration _config;

		private ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

        public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
		{
			_emailRepository = emailRepository;
			_config = configuration;

			_serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString");
            _subscriptionEmail = _config.GetValue<string>("SubscriptionName");
			_orderUpdatePaymentResultTopic = _config.GetValue<string>("OrderUpdatePaymentResultTopic");

			// Creating processor
			var client = new ServiceBusClient(_serviceBusConnectionString);
			_orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _subscriptionEmail);
        }

		public async Task Start()
		{
            _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentMessageRecieved;
            _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await _orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

		private async Task OnOrderPaymentMessageRecieved(ProcessMessageEventArgs args)
		{
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            UpdatePaymentResultMessage objEmail = null;

            if (body is not null)
            {
                objEmail = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);
            }

            try
            {
                await _emailRepository.SendAndLogEmail(objEmail);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}

