using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {
        List<TblCustomer> getCompany();
        ResponseModel addCompany(CustomerModel tblcustomer);
        ResponseModel EditCompany(CustomerModel customer);
        ResponseModel DeleteArrCompany(List<string> deleteIds);
        ResponseModel DeleteCompany(string cid);

        TblCustomer getIDCompany(string cid);

        TblCustomer GetCompanyByCid(string Cid);

    }
    public class CompanyService : ICompanyService
    {
        DataContext db = new DataContext();
        IHttpContextAccessor _httpContextAccessor;
        public CompanyService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public TblCustomer getIDCompany(string cid)
        {
            var companybycid = db.TblCustomer.Where(p => p.Cid == cid && p.DeletedOn == null).FirstOrDefault();
            return companybycid;
        }
        public List<TblCustomer> getCompany()
        {
            var context = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            {
                var company = db.TblCustomer.Where(p => p.DeletedOn == null).ToList();
                return company;
            }
            return null;
        }
        public TblCustomer GetCompanyByCid(string Cid)
        {
                var companybycid = db.TblCustomer.Where(p => p.Cid == Cid && p.DeletedOn == null).FirstOrDefault();
                return companybycid;
            
        }

        public ResponseModel addCompany(CustomerModel tblcustomer)
        {
            var context = _httpContextAccessor.HttpContext;
            Guid g;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            //{
            {
                    
                    try
                    {
                        var listcustomer = db.TblCustomer.FirstOrDefault(p => p.Cid != tblcustomer.Cid);
                        if (listcustomer != null)
                        {
                            g = Guid.NewGuid();
                            var item = new TblCustomer
                            {
                                Cid = g.ToString(),
                                Name = tblcustomer.Name,
                                Address = tblcustomer.Address,
                                Status = tblcustomer.Status,
                                CreateOn = DateTime.UtcNow
                        };
                            db.TblCustomer.Add(item);
                            db.SaveChanges();
                            var model = new CustomerModel
                            {
                                Cid = item.Cid,
                                Name = item.Name,
                                Address = item.Address,
                                Status = item.Status,
                                CreateOn = new DateTime(item.CreateOn.Value.Ticks).ToString("o")
                            };
                            res.Data = model;
                        }

                    }
                    catch (Exception ex)
                    {
                        res.Status = "500";
                        res.Message = ex.Message;
                    }

                    return res;
               // }
            }
            return res;
        }
        public ResponseModel DeleteCompany(string cid)
        {
            var cont = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.superadmin)
            {
                
                try
                {
                    using (DataContext context = new DataContext())
                    {
                        var rs = context.TblCustomer.Where(o => o.Cid == cid).FirstOrDefault();
                        if (rs != null)
                        {
                            rs.DeletedOn = DateTime.UtcNow;
                            context.SaveChanges();
                        }
                        res.Data = rs;
                    }
                }
                catch (Exception ex)
                {
                    res.Status = "500";
                    res.Message = ex.Message;
                }
                return res;
            }
            return res;
        }

        public ResponseModel DeleteArrCompany(List<string> deleteIds)
        {
            var cont = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.superadmin)
            {
                
                try
                {
                    using (DataContext context = new DataContext())
                    {
                        foreach (var deleteId in deleteIds)
                        {
                            var customer = context.TblCustomer.FirstOrDefault(o => o.Cid == deleteId);
                            if (customer != null)
                            {
                                customer.DeletedOn = DateTime.UtcNow;
                            }
                        }
                        res.Data = deleteIds;
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
            return res;
        }
        public ResponseModel EditCompany(CustomerModel customer)
        {
            var cont = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            if (UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(cont.User.Identity.Name)) == UsersConstant.superadmin)
            {
               
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
            return res;
        }
    }
}
