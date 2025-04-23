using SalesForceFunctionApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceFunctionApp.Services.Interfaces
{
	public interface IAuthenticationService
	{
		Task<AuthenticationDTO> Authenticate();
	}
}
