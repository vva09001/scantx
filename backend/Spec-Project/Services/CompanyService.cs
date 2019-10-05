using Spec_Project.Models;
using System;
using Spec_Project.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Scanx.Common;

namespace Spec_Project.Services
{
    public interface ICompanyService
    {
        ResponseModel GetCompany();
        ResponseModel AddCompany(CustomerModel tblcustomer);
        ResponseModel EditCompany(CustomerModel customer);
        ResponseModel DeleteArrCompany(List<string> deleteIds);
        ResponseModel DeleteCompany(string cid);
        //TblCustomer GetCompanyByCid(string Cid);
        ResponseModel AddUserToCompany(string usersname, string email, string cid);

    }
    public class CompanyService : ICompanyService
    {
        IHttpContextAccessor _httpContextAccessor;
        IUserService _userService;
        public CompanyService(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public ResponseModel GetCompany()
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var context = _httpContextAccessor.HttpContext;
            using (DataContext _context = new DataContext())
            {
                var role = _userService.GetRole(int.Parse(context.User.Identity.Name));
                if (role == Constant.Users.Superadmin)
                {
                    var userss = _context.TblCustomer.Where(p => p.DeletedOn == null).Select(p => new CustomerModel
                    {
                        Name = p.Name,
                        Address = p.Address,
                        Status = p.Status,
                        Cid = p.Cid,
                    }).ToList();
                    res.Data = userss;
                }
                else
                {
                    if (role == Constant.Users.Admin || role == Constant.Users.User)
                    {
                        var ciduser = _userService.GetCID(context.User.Identity.Name);
                        var user = _context.TblCustomer.Where(p => p.DeletedOn == null && p.Cid == ciduser).Select(p => new CustomerModel
                        {
                            Name = p.Name,
                            Address = p.Address,
                            Status = p.Status,
                            Cid = p.Cid,
                        }).ToList();
                        res.Data = user;
                    }
                }
            }
            return res;
        }

        //public TblCustomer GetCompanyByCid(string Cid)
        //{
        //        var companybycid = db.TblCustomer.Where(p => p.Cid == Cid && p.DeletedOn == null).FirstOrDefault();
        //        return companybycid;

        //}

        public ResponseModel AddCompany(CustomerModel tblcustomer)
        {
            var context = _httpContextAccessor.HttpContext;
            Guid g;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            var role = _userService.GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
            {

                try
                {
                    using (var db = new DataContext())
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

        public ResponseModel AddUserToCompany(string userName, string email, string cid)
        {
            var context = _httpContextAccessor.HttpContext;
            var res = new ResponseModel()
            {
                Status = "200",
                Message = "",
            };
            var role = _userService.GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
            {
                try
                {
                    using (var db = new DataContext())
                    {
                        var checkuser = db.TblUsers.FirstOrDefault(p => p.UserName == userName && p.Email == email);
                        if (checkuser != null)
                        {
                            checkuser.Cid = cid;
                            db.SaveChanges();
                            res.Message = "sucessfull";
                        }
                        else
                        {
                            res.Message = "Username && Email not found !";
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
            var role = _userService.GetRole(int.Parse(cont.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            var role = _userService.GetRole(int.Parse(cont.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            var role = _userService.GetRole(int.Parse(cont.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
            {

                try
                {
                    using (var db = new DataContext())
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
