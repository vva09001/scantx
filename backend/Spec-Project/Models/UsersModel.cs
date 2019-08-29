using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Models
{
    public class UsersModel
    {
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string Company { get; set; }
        public string Authorization { get; set; } //role
    }
}
