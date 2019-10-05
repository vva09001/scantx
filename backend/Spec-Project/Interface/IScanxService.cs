using Scanx.Common;
using Scanx.Web.Models;
using System.ServiceModel;

namespace Scanx.Web.Interface
{
    [ServiceContract]
    public interface IScanxService
    {
        [OperationContract]
        ResponseSOAPModel ImportScanData(ImportDataModel inputModel);

    }
}
