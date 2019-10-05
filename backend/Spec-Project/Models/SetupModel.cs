using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scanx.Web.Models
{
    public class SetupModel
    {
        bool receiveScanData { get; set; }
        bool receiveFile { get; set; }
        bool gateways { get; set; }
    }
}
