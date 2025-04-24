using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SalesForceFunctionApp.DTOs;
using SalesForceFunctionApp.Models;
using SalesForceFunctionApp.Services.Interfaces;

namespace SalesForceFunctionApp.Functions
{
    public class GetAllSalesForce
    {
        private readonly ILogger _logger;
        private readonly ISalesForceService _salesForceService;
        private readonly IServiceBusService _serviceBusService;
        private readonly IAuthenticationService _authenticationService;

        public GetAllSalesForce(ILoggerFactory loggerFactory, ISalesForceService salesForceService, IServiceBusService serviceBusService, IAuthenticationService authenticationService)
        {
            _logger = loggerFactory.CreateLogger<GetAllSalesForce>();
            _salesForceService = salesForceService;
            _serviceBusService = serviceBusService;
            _authenticationService = authenticationService;
        }

        // triggered every 30 minutes. Decrease the interval to test the function faster.
        [Function("GetAllSalesForce")]
        public async Task RunAsync([TimerTrigger("0 */30 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
           
            // Authenticate with SalesForce
            AuthenticationDTO res = await _authenticationService.Authenticate();
            if (res != null)
            {
                _logger.LogInformation($"access token in trigger function: {res.AccessToken}");
                IEnumerable<Account> accounts = await _salesForceService.FetchSalesForceData(res.AccessToken);
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
            else
            {
				_logger.LogError("Authentication Failed.");
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
