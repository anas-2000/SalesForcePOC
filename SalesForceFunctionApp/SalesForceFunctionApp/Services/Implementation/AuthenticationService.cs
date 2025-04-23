using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SalesForceFunctionApp.DTOs;
using SalesForceFunctionApp.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Implementation
{
	public class AuthenticationService:IAuthenticationService
	{
		private readonly ILogger<AuthenticationService> _logger;
		private readonly HttpClient _httpClient;
		private readonly string _clientId;
		private readonly string _clientsecret;
		private readonly string _username;
		private readonly string _password;
		private readonly string _endpoint;

		public AuthenticationService(IConfiguration configuration, HttpClient httpClient, ILogger<AuthenticationService> logger)
        {
			_logger = logger;
			_httpClient = httpClient;
			_clientId = configuration.GetValue<string>("ClientId");
			_clientsecret = configuration.GetValue<string>("ClientSecret");
			_username = configuration.GetValue<string>("Accountname");
			_password = configuration.GetValue<string>("Password");
			_endpoint = configuration.GetValue<string>("OAuthEndPoint");
		}
		public async Task<AuthenticationDTO> Authenticate()
		{
			// contruct query parameters for the request
			string queryParams = "grant_type=password&client_id=" + _clientId + "&client_secret=" + _clientsecret + "&username=" 
				+ _username + "&password=" + _password;

			try
			{
				// final url
				string url = _endpoint + queryParams;
				_logger.LogInformation($"URI: {url}");
				// According to Salesforce docs, this API requires an empty request body and all
				// the information must be passed in the query parameters.
				var response = await _httpClient.PostAsync(url, null);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogInformation($"Response from SalesForce: {content}");
					
					var auth = JsonConvert.DeserializeObject<AuthenticationDTO>(content);

					return auth;
				}
				else
				{
					_logger.LogError($"Invalid credentials: {response.StatusCode}");
					return null;
				}	
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error Authenticating: {ex.Message}");
				return null;
			}

		}
	}
}
