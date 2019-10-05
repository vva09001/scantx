using Models;
using Scanx.Common;
using Scanx.Soap.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server
{
	public class ScanxService : IScanxService
    {
		public string Ping(string s)
		{
			Console.WriteLine("Exec ping method");
			return s;
		}

		public ResponseModel ImportScanData(ImportDataModel postData)
		{
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };

            // check role, login info.
            using (var db = new MiraclesContext())
            {
                var user = db.TblUsers.FirstOrDefault(o => o.UserName == postData.User);
                #region Validate
                if (user == null)
                {
                    res.Message = "Account with user name " + postData.User + " is not existed";
                    res.Status = "500";
                    return res;
                }
                if (user.Token != postData.Token)
                {
                    res.Message = "Token is not correct";
                    res.Status = "500";
                    return res;
                }
                if (user.RoleId != Constant.Users.Admin && user.RoleId != Constant.Users.User && user.RoleId != Constant.Users.Superadmin)
                {
                    res.Message = "You dont have permission to do.";
                    res.Status = "500";
                    return res;
                }
                #endregion
                try
                {
                    Guid g;
                    g = Guid.NewGuid();
                    var x = new TblScanData
                    {
                        ScanId = g.ToString(),
                        Uid = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        StationName = postData.StationName,
                        Payload = postData.ScanData.Payload,
                        DataType = postData.ScanData.DataType,
                        FileName = postData.ScanData.FileName,
                        Status = postData.ScanData.Status,
                        DeletedOn = null,
                    };
                    db.TblScanData.Add(x);
                    db.SaveChanges();
                    x.U = null;
                    res.Data = x;
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
            }
            return res;
        }

		public void VoidMethod(out string s)
		{
			s = "Value from server";
		}

		public Task<int> AsyncMethod()
		{
			return Task.Run(() => 42);
		}

		public int? NullableMethod(bool? arg)
		{
			return null;
		}

		public void XmlMethod(XElement xml)
		{
			Console.WriteLine(xml.ToString());
		}
    }
}
