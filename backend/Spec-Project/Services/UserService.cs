using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spec_Project.Entities;
using Spec_Project.Helpers;
using Spec_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spec_Project.Services
{
    public interface IUserService
    {
        TblUsers Authenticate(string username, string password);





        List<TblUsers> GetAll();
        List<TblUsers> getUser();
        ResponseModel GetById(int id);
        ResponseModel Create(UserDto user, string password, string UserIDLogin);

        ResponseModel Update(TblUsers user, string password = null);
        ResponseModel UpdateNoPass(TblUsers userParam);
        ResponseModel Delete(int id);
        ResponseModel DeleteArrUser(List<int> deleteIds);

    }

    public class UserService : IUserService
    {
        DataContext _context;
        IHttpContextAccessor _httpContextAccessor;


        public UserService(IHttpContextAccessor httpContextAccessor, DataContext context)

        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public TblUsers Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.TblUsers.FirstOrDefault(o => o.DeletedOn == null && o.UserName == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!IsCorrectPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public List<TblUsers> GetAll()
        {
            var data = _context.TblUsers.ToList();
            return data;
        }


        public List<TblUsers> getUser()
        {
            var context = _httpContextAccessor.HttpContext;
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            {
                var userss = _context.TblUsers.Where(p => p.DeletedOn == null).ToList();
                return userss;
            }
            else {
                if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.user)
                {
                    var user = _context.TblUsers.Where(p => p.Id == int.Parse(context.User.Identity.Name)).ToList();
                    return user;
                }
            }
            return null;
           
        }

        public ResponseModel GetById(int id)
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            try
            {
                var x = _context.TblUsers.Where(o => o.DeletedOn == null && o.Id == id).FirstOrDefault();
                res.Data = x;
            }
            catch (Exception ex)
            {
                res.Message = "id khong ton tai";
            }
            return res;
        }

        public ResponseModel Create(UserDto userNew, string password, string UserIDLogin)
        {

            //if (user.TypeOfAccount == TypeOfAccConstant.Private || user.TypeOfAccount == TypeOfAccConstant.Test)
            //{

            //}
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });

            var context = _httpContextAccessor.HttpContext;
            //if (int.Parse(UsersConstant.GetRole(context.User.Identity.Name)) != RoleConstant.reader)
            //{
            //var userlogin = _context.TblUsers.Where(o=> o.Id == int.Parse(UserIDLogin)).FirstOrDefault();
            //userNew.Cid = userlogin.Cid;
            //userNew.TypeOfAccount = "Commercial";


            if (userNew.TypeOfAccount == "Private" || userNew.TypeOfAccount == "Test")
            {
                try
                {
                    CompanyService cpns = new CompanyService(_httpContextAccessor);
                    cpns.addCompany(new CustomerModel
                    {
                        Name = userNew.UserName
                    });
                }
                catch (Exception ex)
                {
                    return res = new ResponseModel
                    {
                        Data = "",
                        Status = "500",
                        Message = ex.Message
                    };
                }
            
            if (userNew.TypeOfAccount == "Commercial")
            {
                CompanyService cp = new CompanyService(_httpContextAccessor);
                var newcp = new CustomerModel
                {
                    Name = "sasd"
                };


                var tbluser = new TblUsers
                {

                    UserName = userNew.UserName,
                    FamilyName = userNew.FamilyName,
                    GivenName = userNew.GivenName,
                    TypeOfAccount = userNew.TypeOfAccount,
                    Email = userNew.Email,
                    ContactByEmail = userNew.ContactByEmail,
                    EncryptionActive = userNew.EncryptionActive,
                    DeletedOn = null,
                    Cid = _context.TblCustomer.Where(o => o.Name == userNew.UserName).Select(o => o.Cid).FirstOrDefault(),
                    RoleId = userNew.RoleID
                };

                try
                {


                    _context.TblUsers.Add(tbluser);
                    _context.SaveChanges();
                    res.Data = tbluser;

                    // validation
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = "Password is required";
                        return res;

                    }

                    if (_context.TblUsers.Any(x => x.UserName == userNew.UserName && x.DeletedOn == null))
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = "Username \"" + userNew.UserName + "\" is already taken";
                        return res;
                    }


                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    tbluser.PasswordHash = passwordHash;
                    tbluser.PasswordSalt = passwordSalt;

                    _context.TblUsers.Add(tbluser);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = ex.Message;
                }

            }
            }
            return res;

        }


        public ResponseModel Update(TblUsers userParam, string password = null)
        {
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            {
                var user = _context.TblUsers.Where(o => o.Id == userParam.Id && o.DeletedOn == null).FirstOrDefault();

                
                try
                {
                    if (user == null)
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = "User not found";
                        return res;
                    }

                    if (userParam.UserName != user.UserName)
                    {
                        // username has changed so check if the new username is already taken
                        if (_context.TblUsers.Any(x => x.UserName == userParam.UserName))
                        {
                            res.Data = "";
                            res.Status = "500";
                            res.Message = ("Username " + userParam.UserName + " is already taken");
                        }

                    }

                    // update user properties

                    user.FamilyName = userParam.FamilyName;
                    user.GivenName = userParam.GivenName;
                    user.UserName = userParam.UserName;
                    user.TypeOfAccount = userParam.TypeOfAccount;
                    user.PasswordHash = userParam.PasswordHash;
                    user.PasswordSalt = userParam.PasswordSalt;
                    user.Email = userParam.Email;
                    user.Cid = userParam.Cid;
                    user.RoleId = userParam.RoleId;
                    user.ContactByEmail = userParam.ContactByEmail;
                    user.EncryptionActive = userParam.EncryptionActive;

                    // update password if it was entered
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        byte[] passwordHash, passwordSalt;
                        using (var hmac = new System.Security.Cryptography.HMACSHA512())
                        {
                            passwordSalt = hmac.Key;
                            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                        }

                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                    }

                    _context.TblUsers.Update(user);
                    _context.SaveChanges();
                    res.Data = userParam;

                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = ("Username " + userParam.UserName + " is already taken");

                }
                if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.user)
                {
                    var user2 = _context.TblUsers.Where(o => o.Id == int.Parse(context.User.Identity.Name) && o.DeletedOn == null).FirstOrDefault();

                    ResponseModel res2 = (new ResponseModel
                    {
                        Data = "",
                        Status = "200",
                        Message = ""
                    });
                    try
                    {
                        if (user2 == null)
                        {
                            res.Data = "";
                            res.Status = "500";
                            res.Message = "User not found";
                            return res;
                        }

                        if (userParam.UserName != user2.UserName)
                        {
                            // username has changed so check if the new username is already taken
                            if (_context.TblUsers.Any(x => x.UserName == userParam.UserName))
                            {
                                res.Data = "";
                                res.Status = "500";
                                res.Message = ("Username " + userParam.UserName + " is already taken");
                            }

                        }

                        // update user properties

                        user2.FamilyName = userParam.FamilyName;
                        user2.GivenName = userParam.GivenName;
                        user2.UserName = userParam.UserName;
                        user2.TypeOfAccount = userParam.TypeOfAccount;
                        user2.PasswordHash = userParam.PasswordHash;
                        user2.PasswordSalt = userParam.PasswordSalt;
                        user2.Email = userParam.Email;
                        user2.Cid = userParam.Cid;
                        user2.RoleId = userParam.RoleId;
                        user2.ContactByEmail = userParam.ContactByEmail;
                        user2.EncryptionActive = userParam.EncryptionActive;

                        // update password if it was entered
                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            byte[] passwordHash, passwordSalt;
                            using (var hmac = new System.Security.Cryptography.HMACSHA512())
                            {
                                passwordSalt = hmac.Key;
                                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                            }

                            user2.PasswordHash = passwordHash;
                            user2.PasswordSalt = passwordSalt;
                        }

                        _context.TblUsers.Update(user2);
                        _context.SaveChanges();
                        res.Data = userParam;

                    }
                    catch (Exception ex)
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = ("Username " + userParam.UserName + " is already taken");

                    }
                    return res;
                }
            }
            return res;
        }

        public ResponseModel UpdateNoPass(TblUsers userParam)
        {
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            {
                var user = _context.TblUsers.Where(o => o.Id == userParam.Id && o.DeletedOn == null).FirstOrDefault();

                
                try
                {
                    if (user == null)
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = "User not found";
                        return res;
                    }

                    if (userParam.UserName != user.UserName)
                    {
                        // username has changed so check if the new username is already taken
                        if (_context.TblUsers.Any(x => x.UserName == userParam.UserName))
                        {
                            res.Data = "";
                            res.Status = "500";
                            res.Message = ("Username " + userParam.UserName + " is already taken");
                        }

                    }
                    // update user properties

                    user.FamilyName = userParam.FamilyName;
                    user.GivenName = userParam.GivenName;
                    user.UserName = userParam.UserName;
                    user.TypeOfAccount = userParam.TypeOfAccount;
                    user.Email = userParam.Email;
                    user.Cid = userParam.Cid;
                    user.RoleId = userParam.RoleId;
                    user.ContactByEmail = userParam.ContactByEmail;
                    user.EncryptionActive = userParam.EncryptionActive;
                    _context.TblUsers.Update(user);
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = ("Username " + userParam.UserName + " is already taken");

                }
                return res;
            }
            return res;
        }

        internal static void CreatePasswordHash(string password, out object passwordH, out object passwordS)
        {
            throw new NotImplementedException();
        }

        public ResponseModel Delete(int id)
        {

            var user = _context.TblUsers.Where(o => o.DeletedOn == null).FirstOrDefault(o => o.Id == id);
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            if (UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.admin || UsersConstant.GetRole(int.Parse(context.User.Identity.Name)) == UsersConstant.superadmin)
            {
                if (user != null)
                {
                    user.DeletedOn = DateTime.UtcNow;
                    //_context.TblUsers.Remove(user);
                    _context.SaveChanges();
                    res.Status = "200";
                    res.Message = "";
                    res.Data = user;
                }
                return res;
            }
            return res;
        }

        // private helper methods

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool IsCorrectPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public ResponseModel DeleteArrUser(List<int> deleteIds)
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
                            var user = context.TblUsers.FirstOrDefault(o => o.Id == deleteId);
                            if (user != null)
                            {
                                user.DeletedOn = DateTime.UtcNow;
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
    }
}


