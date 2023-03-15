using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Newtonsoft.Json;

namespace Mango.Services.OrderAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly OrderRepository _orderRepository;
		private readonly string _serviceBusConnectionString;
        private readonly string _checkoutMessageTopic;
        private readonly string _subscriptionNameCheckout;
		private readonly string _orderPaymentProcessTopic;
		private readonly string _orderUpdatePaymentResultTopic;

        private readonly IConfiguration _config;

		private ServiceBusProcessor _checkoutProcessor;
		private ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

		private IMessageBus _messageBus; 

        public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
		{
			_orderRepository = orderRepository;
			_config = configuration;

			_serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString");
			_checkoutMessageTopic = _config.GetValue<string>("CheckoutMessageTopic");
            _subscriptionNameCheckout = _config.GetValue<string>("SubscriptionCheckout");
			_orderPaymentProcessTopic = _config.GetValue<string>("OrderPaymentProcessTopic");
			_orderUpdatePaymentResultTopic = _config.GetValue<string>("OrderUpdatePaymentResultTopic");

			_messageBus = messageBus;

			// Creating processor
			var client = new ServiceBusClient(_serviceBusConnectionString);
			_checkoutProcessor = client.CreateProcessor(_checkoutMessageTopic, _subscriptionNameCheckout);
			_orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _subscriptionNameCheckout);
        }

		public async Task Start()
		{
			_checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageRecieved;
			_checkoutProcessor.ProcessErrorAsync += ErrorHandler;
			await _checkoutProcessor.StartProcessingAsync();

            _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentMessageRecieved;
            _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _checkoutProcessor.StopProcessingAsync();
			await _checkoutProcessor.DisposeAsync();

            await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await _orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

        private async Task OnCheckoutMessageRecieved(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);
			CheckoutHeaderDto checkoutHeader = null;

			if(body is not null)
			{
				checkoutHeader = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);
			}

			// Can be done using automapper too
			var orderHeader = new OrderHeader()
			{
				UserId = checkoutHeader.UserId,
				FirstName = checkoutHeader.FirstName,
				LastName = checkoutHeader.LastName,
				OrderDetails = new List<OrderDetails>(),
				CardNumber = checkoutHeader.CardNumber,
				CouponCode = checkoutHeader.CouponCode,
				CVV = checkoutHeader.CVV,
				DiscountTotal = checkoutHeader.DiscountTotal,
				Email = checkoutHeader.Email,
				ExpireMonthYear = checkoutHeader.ExpireMonthYear,
				OrderTime = DateTime.Now,
				OrderTotal = checkoutHeader.OrderTotal,
				PaymentStatus = false,
				Phone = checkoutHeader.Phone,
				PickupDateTime = checkoutHeader.PickupDateTime
			};

			// Adding order details
			foreach(var details in checkoutHeader.CartDetails)
			{
				OrderDetails orderDetails = new()
				{
					ProductId = details.ProductId,
					ProductName = details.Product.name,
					Price = details.Product.price,
					Count = details.Count
				};

				orderHeader.CartTotalItems += details.Count;
				orderHeader.OrderDetails.Add(orderDetails);
			}

			await _orderRepository.AddOrder(orderHeader);

			// Adding Payment Details
			var paymentMessage = new PaymentRequestMessage
			{
				Name = orderHeader.FirstName + " " + orderHeader.LastName,
				CardNumber = orderHeader.CardNumber,
				CVV = orderHeader.CVV,
				ExpiryMonthYear = orderHeader.ExpireMonthYear,
				OrderId = orderHeader.OrderHeaderId,
				OrderTotal = orderHeader.OrderTotal,
				Email = orderHeader.Email
			};

			try
			{
				await _messageBus.PublishMessage(paymentMessage, _orderPaymentProcessTopic);
				await args.CompleteMessageAsync(args.Message);
			}
			catch(Exception e)
			{
				throw;
			}
		}

		private async Task OnOrderPaymentMessageRecieved(ProcessMessageEventArgs args)
		{
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            UpdatePaymentResultMessage paymentResultMessage = null;

            if (body is not null)
            {
                paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);
            }

			await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);
			await args.CompleteMessageAsync(args.Message);
        }

    }
}

