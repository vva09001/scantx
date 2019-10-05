using Scanx.Common;
using System.ServiceModel;

namespace Models
{
	[ServiceContract]
	public interface ISampleService
	{
		[OperationContract]
        ResponseModel ImportScanData(ImportDataModel inputModel);

	}
}
