namespace Scanx.Web.Models
{
    public class QRModel
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
