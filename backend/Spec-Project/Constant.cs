using Spec_Project.Entities;
using System.IO;
using System.Xml.Serialization;

namespace Scanx.Common
{
    public static class ConstantScanData
    {
        public const int received = 0;
        public const int processed = 1;
        public const int failed = 2;
    }

    public static class TypeOfAccConstant
    {
        public const int Private = 1;
        public const int Test = 2;
        public const int Commercial = 3;

    }
}