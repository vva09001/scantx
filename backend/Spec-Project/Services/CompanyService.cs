using Spec_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {

        CompanyModel getCompany();


    }
    public class CompanyService : ICompanyService
    {


      

        public CompanyModel getCompany()
        {
            CompanyModel company = (new CompanyModel
            {
                CID = "HFIE_FNE",
                Name = "CSBGADSSD",
                Adress = "109 Hang Bai",
                Country = "UK",
                Remarks = "true"
            }); ;

            return company;
        }

       

    }
}
