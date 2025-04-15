using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesForceFunctionApp.Models;
using SalesForceFunctionApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Implementation
{
	public class ServiceBusService : IServiceBusService
	{
		private readonly string _connectionString;
		private readonly string _queueUrl;
		private readonly ILogger<ServiceBusService> _logger;
        public ServiceBusService(IConfiguration configuration, ILogger<ServiceBusService> logger)
        {
            _connectionString = configuration.GetValue<string>("ServiceBusConnectionString");
			_queueUrl = configuration.GetValue<string>("QueueUrl");
			_logger = logger;
        }
        public async Task SendMessage(IEnumerable<Account> Accounts)
		{
			var client = new ServiceBusClient(_connectionString);
			var sender = client.CreateSender(_queueUrl);

			try
			{
				await sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(Accounts)));
				_logger.LogInformation("Message sent to Service Bus queue.");
			}
			finally
			{
				await sender.DisposeAsync();
				await client.DisposeAsync();
			}

		}
	}
}
