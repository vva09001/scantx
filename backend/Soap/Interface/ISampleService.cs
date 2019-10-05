using Scanx.Common;
using Scanx.Soap.Models;
using System.ServiceModel;

namespace Scanx.Soap.Interface
{
	[ServiceContract]
	public interface IScanxService
    {
		[OperationContract]
        ResponseSOAPModel ImportScanData(ImportDataModel inputModel);

	}
}
