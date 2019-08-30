using Spec_Project.Entities;
using Spec_Project.Helpers;
using Spec_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Services
{
    public interface IStartService
    {
        //ResponseModel Login(string username, string password );
        List<TblUsers> getUser();
        ScanInputModel getDataScanInput(string userID);
        ResponseModel AddUser();
    }
    public class StartService : IStartService
    {
        DataContext db = new DataContext();

        //public ResponseModel Login(string username, string password)
        //{


        //    var res = (new ResponseModel {
        //        Data = "",
        //        Status = "200",
        //        Message = ""

        //    });
        //    try
        //    {
        //        var rs = db.TblUsers.Where(o => o.UserName == username && o.Password == password).FirstOrDefault();
        //        if (rs != null)
        //        {
        //            res.Data = (new UsersModel
        //            {
        //                Fullname = rs.GivenName + " " + rs.FamilyName,
        //                Mail = rs.Email,
        //                Company = db.TblCustomer.Where(o=>o.Cid == rs.cid)
        //            }) ;
        //            res.Status = "200";
        //            res.Message = "";
        //        }
        //        else
        //        {
        //            res.Data = "Null";
        //            res.Status = "200";
        //            res.Message = "Username or Password incorrect.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Data = "";

        //    }

        //    return res;
        //}

        public List<TblUsers> getUser()
        {
            var listuser = db.TblUsers.Where(p => p.Cid != null).ToList();
            return listuser;
        }

        public ScanInputModel getDataScanInput(string userID)
        {
            List<ScanModel> sc = new List<ScanModel>();
            sc.Add(new ScanModel {
                TimeScan = new DateTime(2019,05,24,14,55,03),
                Code = "471135899"
            });
            ScanInputModel data = (new ScanInputModel
            {
                NumberScan = 194,
                Detail = sc
            });
            return data;
        }

        public ResponseModel AddUser()
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            try {

            }
            catch (Exception ex)
            {
            }
           
            return res;
        }
        public string EditUser()
        {
            string rs = string.Empty;
            return rs = "Success";
        }
    }
}
