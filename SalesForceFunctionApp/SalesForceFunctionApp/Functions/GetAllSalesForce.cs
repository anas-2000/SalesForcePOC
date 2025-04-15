using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalesForceFunctionApp.Models;
using SalesForceFunctionApp.Services.Interfaces;

namespace SalesForceFunctionApp.Functions
{
    public class GetAllSalesForce
    {
        private readonly ILogger _logger;
        private readonly ISalesForceService _salesForceService;
        private readonly IServiceBusService _serviceBusService;

        public GetAllSalesForce(ILoggerFactory loggerFactory, ISalesForceService salesForceService, IServiceBusService serviceBusService)
        {
            _logger = loggerFactory.CreateLogger<GetAllSalesForce>();
            _salesForceService = salesForceService;
            _serviceBusService = serviceBusService;
        }

        [Function("GetAllSalesForce")]
        public async Task RunAsync([TimerTrigger("0 */30 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            // For simplicity, I have stored the access token in the environment variable, however there would be separate
            // api end-point (an HTTP Triggered function) to deal with authentication, and a better approach would be to cache the token,
            // since this functions is triggered every 30 minutes.
            IEnumerable<Account> accounts = await _salesForceService.FetchSalesForceData(Environment.GetEnvironmentVariable("AccessToken"));
            if (accounts != null && accounts.Any())
            {
				_logger.LogInformation($"Fetched {accounts.Count()} accounts from SalesForce.");
				await _serviceBusService.SendMessage(accounts);
			}
			else
            {
				_logger.LogWarning("No accounts fetched from SalesForce.");
			}

        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
