using Scanx.Web.Helpers;
using Scanx.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scanx.Web.Services
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
        private DataContext _context;
        public StartService(DataContext context)
        {
            _context = context;
        }

        public List<TblUsers> getUser()
        {
            var listuser = _context.TblUsers.Where(p => p.Cid != null).ToList();
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
