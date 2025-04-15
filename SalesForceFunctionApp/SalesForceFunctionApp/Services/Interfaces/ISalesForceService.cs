using SalesForceFunctionApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Interfaces
{
	public interface ISalesForceService
	{
		Task<IEnumerable<Account>> FetchSalesForceData(string AccessToken);
	}
}
