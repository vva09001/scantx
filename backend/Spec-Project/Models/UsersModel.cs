using System;

namespace Spec_Project.Models
{
    public class UsersModel
    {
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string Company { get; set; }
        public string Authorization { get; set; } //role
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string TypeOfAccount { get; set; }
        public string Email { get; set; }
        public bool? ContactByEmail { get; set; }
        public bool? EncryptionActive { get; set; }
        public string Cid { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int RoleID { get; set; }
    }
}
