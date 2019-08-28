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
            }) ;
           
            return user;
        }
        public ScanInputModel getDataScanInput(string userID)
        {
            ScanInputModel data = (new ScanInputModel
            {
                timeScan = new DateTime(2019, 5, 24, 14, 35, 03),
                code = "471135899"

            }) ;

            return data;
        }
    }
}
