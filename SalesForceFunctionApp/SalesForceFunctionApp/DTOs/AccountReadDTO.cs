using Newtonsoft.Json;
using SalesForceFunctionApp.Models;
using System.Collections.Generic;


namespace SalesForceFunctionApp.DTOs
{
	public class AccountReadDTO
	{
		[JsonProperty("Records")]
		public IEnumerable<Account> Accounts { get; set; }
	}
}
