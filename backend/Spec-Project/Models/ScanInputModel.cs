using System;
using System.Collections.Generic;

namespace Spec_Project.Models
{
    public class ScanModel
    {

        public DateTime TimeScan { get; set; }
        public string Code { get; set; }
    }
    public class ScanInputModel
    {
        public int NumberScan { get; set; }
        public List<ScanModel> Detail { get; set; }
    }
    public class ListScanDataModel
    {
        public string ScanID { get; set; }
        public string UID { get; set; }
        public string BLOPData { get; set; }
        public string StatusData { get; set; }
    }
}
