using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TypeOfAccount { get; set; }
        public string Email { get; set; }
        public bool? ContactByEmail { get; set; }
        public bool? EncryptionActive { get; set; }
        public string Cid { get; set; }
        public int RoleID { get; set; }

    }
}
