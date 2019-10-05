using System.Runtime.Serialization;

namespace Scanx.Soap.Models
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember]
        public object Data { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
