using Spec_Project.Entities;
using Spec_Project.Models;
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
        public static int GetRole(int pUserID)
        {
            using (DataContext db = new DataContext())
            {
                var roleid = db.TblUsers.Where(o => o.Id == pUserID).Select(o => o.RoleID).FirstOrDefault();
                return roleid;
            }
        }
        public static int GetID(int userID)
        {
            using (DataContext db = new DataContext())
            {
                var roleid = db.TblUsers.Where(o => o.Id == userID).Select(o => o.RoleID).FirstOrDefault();
                return roleid;
            }
        }

        public static string GetCID(string userID)
        {
            var strcid = int.Parse(userID);
            using (DataContext db = new DataContext())
            {
                var cid = db.TblUsers.Where(o => o.Id == strcid).Select(o => o.Cid).FirstOrDefault();
                return cid;
            }
        }
        public static string GetUserName(string userid)
        {
            var userID = int.Parse(userid);
            using (DataContext db = new DataContext())
            {
                var username = db.TblUsers.Where(o => o.Id == userID).Select(o => o.UserName).FirstOrDefault();
                return username;
            }
        }
    }

    public static class TypeOfAccConstant
    {
        public const int Private = 1;
        public const int Test = 2;
        public const int Commercial = 3;

    }
}