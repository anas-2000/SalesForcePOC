using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesForceFunctionApp.Services.Implementation;
using SalesForceFunctionApp.Services.Interfaces;

namespace SalesForceFunctionApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			FunctionsDebugger.Enable();

			var host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.ConfigureServices(services =>
				{
					services.AddHttpClient();
					services.AddScoped<ISalesForceService, SalesForceService>();
					services.AddScoped<IServiceBusService, ServiceBusService>();
				})
				.Build();

			host.Run();
		}
	}
}
