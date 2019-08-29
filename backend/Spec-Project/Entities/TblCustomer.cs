﻿using System;
using System.Collections.Generic;

namespace Spec_Project.Entities
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblUsers = new HashSet<TblUsers>();
        }

        public string Cid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TblUsers> TblUsers { get; set; }

        public static implicit operator string(TblCustomer v)
        {
            throw new NotImplementedException();
        }
    }
}
