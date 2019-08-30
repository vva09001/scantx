using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {
        CompanyModel getCompany();
        string addCompany();
        ResponseModel EditCompany(TblCustomer customer);
        ResponseModel DeleteCompany(string cid);
    }
    public class CompanyService : ICompanyService
    {
        DB_9A9CCA_scantxContext db = new DB_9A9CCA_scantxContext();
        public DB_9A9CCA_scantxContext _content;
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
                _content.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "done";
        }
        public ResponseModel DeleteCompany(string cid)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                var rs = db.TblCustomer.Where(o => o.Cid == cid).FirstOrDefault();
                if (rs!=null)
                {
                    rs.DeletedOn = DateTime.UtcNow.AddHours(7);
                    db.SaveChanges();
                    db.TblCustomer.Remove(rs);
                }
            }
            catch (Exception ex)
            {
                res.Status = "500";
                res.Message = ex.Message;
            }
            return res;
        }
        public ResponseModel EditCompany(TblCustomer customer)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                var oldCusomer = (from u in db.TblCustomer where u.Cid == customer.Cid select u).FirstOrDefault();
                if (oldCusomer != null)
                {
                    oldCusomer.Address = customer.Address;
                    oldCusomer.Name = customer.Name;
                    oldCusomer.Status = customer.Status;
                    db.SaveChanges();
                    res.Data = oldCusomer;
                }
            }
            catch (Exception ex)
            {
                res.Status = "500";
                res.Message = ex.Message;
            }
            return res;
        }
    }
}
