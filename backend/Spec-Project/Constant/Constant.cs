using System.IO;
using System.Xml.Serialization;

namespace MBI.Extension
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
    }
    public static class RoleConstant
    {
        public const int superadmin = 1;
        public const string admin = "admin";
        public const string user = "user";
        public const int reader = 4;
    }
}