using System;
using System.Collections.Generic;

namespace Spec_Project.Entities
{
    public partial class TblScanData
    {
        public string ScanId { get; set; }
        public string Uid { get; set; }
        public string Payload { get; set; }
        public string StatusData { get; set; }
    }
}
