using System;
using Mango.Services.OrderAPI.Messaging;

namespace Mango.Services.OrderAPI.Extension
{
	// This class is for starting our consumer after application has started
	public static class ApplicationBuilderExtensions
	{
		public static IAzureServiceBusConsumer serviceBusConsumer { get; set; }

		public static WebApplication UseAzureServiceBusConsumer(this WebApplication app, IServiceProvider service)
		{
			serviceBusConsumer = service.GetService<IAzureServiceBusConsumer>();
			var hostApplicationLife = service.GetService<IHostApplicationLifetime>();
			
			hostApplicationLife.ApplicationStarted.Register(OnStart);
			hostApplicationLife.ApplicationStopped.Register(OnStop);
			
			return app;
		}

		private static void OnStart()
		{
			serviceBusConsumer.Start();
		}

		private static void OnStop()
		{
			serviceBusConsumer.Stop();
		}
	}
}

