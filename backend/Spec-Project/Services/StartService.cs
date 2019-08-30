using Spec_Project.Entities;
using Spec_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Services
{
    public interface IStartService
    {
        ResponseModel Login(string username, string password);
        ScanInputModel getDataScanInput(string userID);
        ResponseModel AddUser();
    }
    public class StartService : IStartService
    {
        DB_9A9CCA_scantxContext db = new DB_9A9CCA_scantxContext();

        public ResponseModel Login(string username, string password)
        {
            

            var res = (new ResponseModel {
                Data = "",
                Status = "200",
                Message = ""

            });
            try
            {
                var rs = db.TblUsers.Where(o => o.UserName == username && o.Password == password).Select(o=>new TblUsers {
                    
                }).FirstOrDefault();
                if (rs != null)
                {
                    
                }
            }
            catch (Exception ex)
            {

            }
           
            return res;
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
