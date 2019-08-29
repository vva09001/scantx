using Spec_Project.Models;
using System;
using Spec_Project.Entities;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {
        CompanyModel getCompany();
        string addCompany();
        string editCompany();
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
                Status = "active",
            }); ;

            return company;
        }

        public string addCompany()
        {
            try
            {
                DB_9A9CCA_scantxContext db = new DB_9A9CCA_scantxContext();
                string cid = "JSDHOAOCK";
                string name = "CSBGADSSD";
                string adress = "109 Hang Bai";
                string status = "active";
                db.TblCustomer.Add(new TblCustomer
                {
                    Cid = cid,
                    Name = name,
                    Address = adress,
                    Status = status,
                });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "done";
        }
        public string editCompany()
        {
            try
            {
                DB_9A9CCA_scantxContext db = new DB_9A9CCA_scantxContext();
                string cid = "JSDHOAOCK";
                string name = "CSBGADSSD";
                string adress = "109 Hang Bai";
                string status = "active";
                db.TblCustomer.Update(new TblCustomer
                {
                    Cid = cid,
                    Name = name,
                    Address = adress,
                    Status = status,
                });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "done";
        }
    }
}
