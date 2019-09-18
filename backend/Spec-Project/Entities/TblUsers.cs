using System;
using System.Collections.Generic;

namespace Spec_Project.Models
{
    public partial class TblUsers
    {
        public TblUsers()
        {
            TblScanData = new HashSet<TblScanData>();
        }

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
        public DateTime? DeletedOn { get; set; }
        public int RoleID { get; set; }

        public virtual TblCustomer C { get; set; }
        public virtual ICollection<TblScanData> TblScanData { get; set; }
    }
}
