using Scanx.Common;
using Scanx.Web.Interface;
using Scanx.Web.Models;
using System;
using System.Linq;

namespace Scanx.Web.Service
{
    public class ScanxService : IScanxService
    {
		public ResponseSOAPModel ImportScanData(ImportDataModel postData)
		{
            var res = new ResponseSOAPModel()
            {
                Status = "201",
                Message = "",
            };

            // check role, login info.
            using (var db = new DataContext())
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
                    res.Message = "Scan data is created";
                    //res.Data = x;
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
            }
            return res;
        }
    }
}
