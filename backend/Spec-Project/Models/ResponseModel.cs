using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scanx.Web.Models
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
