using Spec_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Services
{
    public interface IStartService
    {
        UsersModel getUser(string userID);
        ScanInputModel getDataScanInput(string userID);
    }
    public class StartService : IStartService
    {


        public UsersModel getUser(string userID)
        {
            UsersModel user = (new UsersModel
            {
                fullname = "Christian Gathmann",
                mail = "gathmann@csdg.de",
                company = "CSBG",
                authorization = "admin/input/reader"
                ,position = "admin"
            }) ;
           
            return user;
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


            }) ;

            return data;
        }
    }
}
