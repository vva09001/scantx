using System.ServiceModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Scanx.Soap.Interface;
using Scanx.Soap.Service;
using SoapCore;

namespace Scanx.Soap
{
    public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.TryAddSingleton<IScanxService, ScanxService>();
			services.AddMvc(x => x.EnableEndpointRouting = false);
			services.AddSoapCore();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseSoapEndpoint<IScanxService>("/Service.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);
			app.UseSoapEndpoint<IScanxService>("/Service.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

			app.UseMvc();
		}
	}
}
