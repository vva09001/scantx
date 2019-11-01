using Scanx.Web.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Scanx.Common;

namespace Scanx.Web.Services
{
    public interface ICompanyService
    {
        ResponseModel GetCompany();
        ResponseModel AddCompany(CustomerModel tblcustomer);
        ResponseModel EditCompany(CustomerModel customer);
        ResponseModel DeleteArrCompany(List<string> deleteIds);
        ResponseModel DeleteCompany(string cid);
        ResponseModel AddUserToCompany(string usersname, string email, string cid);

    }
    public class CompanyService : ICompanyService
    {
        IHttpContextAccessor _httpContextAccessor;
        IUserService _userService;
        private DataContext _context;
        public CompanyService(IHttpContextAccessor httpContextAccessor, IUserService userService, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _context = context;
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
            return res;
        }

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
                    var listcustomer = _context.TblCustomer.FirstOrDefault(p => p.Cid != tblcustomer.Cid);
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
                        _context.TblCustomer.Add(item);
                        _context.SaveChanges();
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
                    var checkuser = _context.TblUsers.FirstOrDefault(p => p.UserName == userName && p.Email == email);
                    if (checkuser != null)
                    {
                        checkuser.Cid = cid;
                        _context.SaveChanges();
                        res.Message = "sucessfull";
                    }
                    else
                    {
                        res.Message = "Username && Email not found !";
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
                    var rs = _context.TblCustomer.Where(o => o.Cid == cid).FirstOrDefault();
                    if (rs != null)
                    {
                        rs.DeletedOn = DateTime.UtcNow;
                        _context.SaveChanges();
                    }
                    res.Data = rs;
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
                    foreach (var deleteId in deleteIds)
                    {
                        var customer = _context.TblCustomer.FirstOrDefault(o => o.Cid == deleteId);
                        if (customer != null)
                        {
                            customer.DeletedOn = DateTime.UtcNow;
                        }
                    }
                    res.Data = deleteIds;
                    _context.SaveChanges();
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
                    var oldCusomer = (from u in _context.TblCustomer where u.Cid == customer.Cid select u).FirstOrDefault();
                    if (oldCusomer != null)
                    {
                        oldCusomer.Address = customer.Address;
                        oldCusomer.Name = customer.Name;
                        oldCusomer.Status = customer.Status;
                        _context.SaveChanges();
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
