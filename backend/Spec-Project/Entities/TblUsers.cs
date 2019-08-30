using System;
using System.Collections.Generic;

namespace Spec_Project.Entities
{
    public partial class TblUsers
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string TypeOfAccount { get; set; }
        public string Authorization { get; set; }
        public string Email { get; set; }
        public bool? ContactByEmail { get; set; }
        public bool? EncryptionActive { get; set; }
        public string Cid { get; set; }

        public virtual TblCustomer C { get; set; }
    }
}
