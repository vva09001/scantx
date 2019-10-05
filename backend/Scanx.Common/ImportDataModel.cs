using System.Runtime.Serialization;

namespace Scanx.Common
{
    [DataContract]
    public class ImportDataModel
    {
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public string Token { get; set; }
        [DataMember]
        public string StationName { get; set; }
        [DataMember]
        public string Command { get; set; }
        [DataMember]
        public ScanDataSoapModel ScanData { get; set; }
    }
}
