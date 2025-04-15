using SalesForceFunctionApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Interfaces
{
	public interface IServiceBusService
	{
		Task SendMessage(IEnumerable<Account> Accounts);

	}
}
