using Spec_Project.Entities;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Spec_Project
{
    public static class ConstantScanData
    {
        public const int received = 0;
        public const int processed = 1;
        public const int failed = 2;
    }
    public static class UsersConstant
    {
        public const int superadmin = 1;
        public const int admin = 2;
        public const int user = 3;
        public const int reader = 4;
        public static string GetRole(string pUserID)
        {
            var userID = int.Parse(pUserID);
            using (DataContext db = new DataContext())
            {
                var roleid = db.TblUsers.Where(o => o.Id == userID).Select(o => o.RoleId).FirstOrDefault();
                return roleid;
            }
        }
        public static string GetID(int userID)
        {
            using (DataContext db = new DataContext())
            {
                var roleid = db.TblUsers.Where(o => o.Id == userID).Select(o => o.RoleId).FirstOrDefault();
                return roleid;
            }
        }
    }
        public static class RoleConstant
        {
            public const int superadmin = 1;
            public const string admin = "admin";
            public const string user = "user";
            public const int reader = 4;
        }
    }