using Scanx.Common;
using System.ServiceModel;

namespace Models
{
	[ServiceContract]
	public interface IScanxService
    {
		[OperationContract]
        ResponseModel ImportScanData(ImportDataModel inputModel);

	}
}
