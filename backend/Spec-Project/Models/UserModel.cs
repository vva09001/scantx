using System;

namespace Scanx.Web.Models
{
    public class UserModel
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string TypeOfAccount { get; set; }
        public string Authorization { get; set; }
        public string Mail { get; set; }
        public bool? ContactByEmail { get; set; }
        public bool? EncryptionActive { get; set; }
        public string Cid { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int RoleID { get; set; }
        public string Token { get; set; }
        public string CompanyName { get; set; }
    }
}
