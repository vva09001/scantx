using System.Runtime.Serialization;

namespace Scanx.Web.Models
{
    [DataContract]
    public class ResponseSOAPModel
    {
        [DataMember]
        public object Data { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
