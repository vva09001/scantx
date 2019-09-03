using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Models
{
    public class CreateQR
    {
        public string Command { get; set; }
        public string ServerAddress { get; set; }
        public int Port { get; set; }
        public string URLPart { get; set; }
        public string EncryptionKey { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
