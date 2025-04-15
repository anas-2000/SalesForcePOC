using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesForceFunctionApp.DTOs;
using SalesForceFunctionApp.Models;
using SalesForceFunctionApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Implementation
{
	public class SalesForceService:ISalesForceService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<SalesForceService> _logger;
		private readonly string _endpoint;

        public SalesForceService(IConfiguration configuration, HttpClient httpClient, ILogger<SalesForceService> logger)
        {
            _logger = logger;
			_httpClient = httpClient;
			_endpoint = configuration.GetValue<string>("EndPoint");
		}

        public async Task<IEnumerable<Account>> FetchSalesForceData(string AccessToken)
		{
			
			// not reading the access token from environment variable here and instead getting it as a parameter, because
			// access token should be received from outside where the service is called.
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

			try
			{

				_logger.LogInformation($"URI: {_endpoint}");

				var response = await _httpClient.GetAsync(_endpoint);


				if (response.IsSuccessStatusCode)
				{ // status code 200
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogInformation($"Response from SalesForce: {content}");
					// deserialize the content to a list of Account objects. 
					// Our current use case only requires json, which means that serializing the response to json
					// would be sufficient, but returning the deserialized object is a good practice for future use cases.
					// and would make our code more generic and scalable.
					// DTO (rather a wrapper around the response) is used to keep only the records array from the response.
					var result = JsonConvert.DeserializeObject<AccountReadDTO>(content);
					return result?.Accounts?? null;
				}
				else
				{
					_logger.LogError($"Error fetching data from SalesForce: {response.StatusCode}");
					return null;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception occurred while fetching data from SalesForce: {ex.Message}");
				return null;
			}
			
		}
	}
	
}
