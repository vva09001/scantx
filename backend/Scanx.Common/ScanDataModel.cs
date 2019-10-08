using System.Runtime.Serialization;

namespace Scanx.Common
{
    public class ScanDataModel
    {
        public string ScanId { get; set; }
        public int? Uid { get; set; }
        public string CreatedOn { get; set; }
        public string Payload { get; set; }
        public string DataType { get; set; }
        public string FileName { get; set; }
        public int? Status { get; set; }
    }
    [DataContract]
    public class ScanDataSoapModel
    {
        [DataMember]
        public string Payload { get; set; }
        [DataMember]
        public string DataType { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int? Status { get; set; }
    }
}
