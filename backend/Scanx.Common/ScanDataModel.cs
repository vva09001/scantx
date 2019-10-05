namespace Scanx.Common
{
    public class ScanDataModel
    {
        public string ScanId { get; set; }
        public int Uid { get; set; }
        public string CreatedOn { get; set; }
        public string Payload { get; set; }
        public string DataType { get; set; }
        public string FileName { get; set; }
        public int? Status { get; set; }
    }
    public class ScanDataSoapModel
    {
        public string Payload { get; set; }
        public string DataType { get; set; }
        public string FileName { get; set; }
        public int? Status { get; set; }
    }
    public class ImportDataModel
    {
        public string User { get; set; }
        public string Token { get; set; }
        public string StationName { get; set; }
        public string Command { get; set; }
        public ScanDataSoapModel ScanData { get; set; }
    }
}
