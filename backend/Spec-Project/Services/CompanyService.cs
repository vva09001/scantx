using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {
        List<TblCustomer> getCompany();
        ResponseModel addCompany(TblCustomer tblcustomer);
        ResponseModel EditCompany(TblCustomer customer);
        ResponseModel DeleteArrCompany(List<string> deleteIds);
        ResponseModel DeleteCompany(string cid);
    }
    public class CompanyService : ICompanyService
    {
        DataContext db = new DataContext();
        public List<TblCustomer> getCompany()
        {
            var company = db.TblCustomer.Where(p => p.DeletedOn == null).ToList();
            return company;
        }

        public ResponseModel addCompany(TblCustomer tblcustomer)
        {
            Guid g;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                var listcustomer = db.TblCustomer.FirstOrDefault(p => p.Cid != tblcustomer.Cid);
                if (listcustomer != null)
                {
                    g = Guid.NewGuid();
                    db.TblCustomer.Add(new TblCustomer
                    {
                        Cid = g.ToString(),
                        Name = tblcustomer.Name,
                        Address = tblcustomer.Address,
                        Status = tblcustomer.Status
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                res.Status = "500";
                res.Message = ex.Message;
            }

            return res;
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
                using (DataContext context = new DataContext())
                {
                    var rs = context.TblCustomer.Where(o => o.Cid == cid).FirstOrDefault();
                    if (rs != null)
                    {
                        rs.DeletedOn = DateTime.UtcNow.AddHours(7);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = "500";
                res.Message = ex.Message;
            }
            return res;
        }

        public ResponseModel DeleteArrCompany(List<string> deleteIds)
        {
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            try
            {
                using (DataContext context = new DataContext())
                {
                    foreach(var deleteId in deleteIds)
                    {
                        var customer = context.TblCustomer.FirstOrDefault(o => o.Cid == deleteId);
                        if(customer != null)
                        {
                            customer.DeletedOn = DateTime.UtcNow.AddHours(7);
                        }
                    }
                    context.SaveChanges();
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
