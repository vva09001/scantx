using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Scanx.Common;
using Scanx.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Scanx.Web.Services
{
    public interface IUserService
    {
        UserModel Authenticate(string username, string password, Helpers.AppSettings appSettings);
        List<TblUsers> GetAll();
        ResponseModel GetUser();
        TblUsers GetUser(string userid);
        ResponseModel GetById(int id);
        ResponseModel Create(UserDto user, string password, string UserIDLogin);
        ResponseModel AddUser(UserDto userNew, string password, string UserIDLogin);

        ResponseModel Update(UsersModel userDto, string password = null);
        ResponseModel UpdateNoPass(UsersModel userDto);
        ResponseModel Delete(int id);
        ResponseModel DeleteArrUser(List<int> deleteIds);
        int GetRole(int pUserID);
        string GetCID(string userID);
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

        public UserModel Authenticate(string username, string password, Helpers.AppSettings appSettings)
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

            var tokenHandler = new JwtSecurityTokenHandler();
            if (appSettings.Secret == null)
            {
                appSettings.Secret = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJmaXJzdG5hbWUiOiJ0dXllbiIsImxhc3RuYW1lIjoibmd1eWVuIiwidXNlcm5hbWUiOiJnZWFyMjE5IiwicGFzc3dvcmQiOiIxMjMifQ.a9jA6viURjMOzqeT58R39ORgmNovPp0OkbGp9VkaoVg";
            }
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            user.Token = tokenString.Substring(0, 25);
            _context.Update(user);
            _context.SaveChanges();
            var companyName = string.Empty;
            var company = _context.TblCustomer.FirstOrDefault(p => p.Cid == user.Cid);
            if (company != null)
            {
                companyName = company.Name;
            }
            return new UserModel
            {
                Id = user.Id,
                Username = user.UserName,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                CompanyName = companyName,
                Mail = user.Email,
                TypeOfAccount = user.TypeOfAccount,
                RoleID = user.RoleId,
                Authorization = user.Authorization,
                Token = tokenString
            };
        }

        public List<TblUsers> GetAll()
        {
            var data = _context.TblUsers.ToList();
            return data;
        }


        public ResponseModel GetUser()
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var context = _httpContextAccessor.HttpContext;
            var role = GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Superadmin)
            {
                var userss = _context.TblUsers.Where(p => p.DeletedOn == null).Select(p => new UsersModel
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    GivenName = p.GivenName,
                    FamilyName = p.FamilyName,
                    TypeOfAccount = p.TypeOfAccount,
                    Email = p.Email,
                    Cid = p.Cid,
                    ContactByEmail = p.ContactByEmail,
                    EncryptionActive = p.EncryptionActive,
                    RoleID = p.RoleId
                }).ToList();
                res.Data = userss;
            }
            else
            {
                if (role == Constant.Users.Admin || role == Constant.Users.User)
                {
                    var ciduser = GetCID(context.User.Identity.Name);
                    var user = _context.TblUsers.Where(p => p.DeletedOn == null && p.Cid == ciduser).Select(p => new UsersModel
                    {
                        Id = p.Id,
                        UserName = p.UserName,
                        GivenName = p.GivenName,
                        FamilyName = p.FamilyName,
                        TypeOfAccount = p.TypeOfAccount,
                        Email = p.Email,
                        Cid = p.Cid,
                        ContactByEmail = p.ContactByEmail,
                        EncryptionActive = p.EncryptionActive,
                        RoleID = p.RoleId
                    }).ToList();
                    res.Data = user;
                }
            }
            return res;
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
                res.Message = "ID not found";
            }
            return res;
        }

        public ResponseModel Create(UserDto userNew, string password, string UserIDLogin)
        {
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });

            var context = _httpContextAccessor.HttpContext;
            if (userNew.TypeOfAccount == "Private" || userNew.TypeOfAccount == "Test")
            {
                Guid g = Guid.NewGuid();

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
                    Cid = g.ToString(),
                    RoleId = userNew.RoleID
                };
                var cpn = new TblCustomer()
                {
                    Cid = tbluser.Cid,
                    Name = tbluser.UserName,
                    CreateOn = DateTime.UtcNow
                };
                try
                {
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

                    _context.TblCustomer.Add(cpn);
                    _context.TblUsers.Add(tbluser);
                    _context.SaveChanges();
                    //res.Data = tbluser;
                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = ex.Message;
                }
            }
            if (userNew.TypeOfAccount == "Commercial")
            {
                Guid g = Guid.NewGuid();

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
                    Cid = g.ToString(),
                    RoleId = userNew.RoleID
                };
                var cpn = new TblCustomer()
                {
                    Cid = tbluser.Cid,
                    Name = userNew.Name,
                    Address = userNew.Address,
                    Status = userNew.Status,
                    CreateOn = DateTime.UtcNow,
                };
                try
                {
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

                    _context.TblCustomer.Add(cpn);
                    _context.TblUsers.Add(tbluser);
                    _context.SaveChanges();
                    //res.Data = tbluser;
                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = ex.Message;
                }
            }
            return res;
        }

        public ResponseModel AddUser(UserDto userNew, string password, string UserIDLogin)
        {
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
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
                Cid = GetCID(context.User.Identity.Name),
                RoleId = userNew.RoleID
            };
            try
            {
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
                res.Data = tbluser;

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.Status = "500";
                res.Message = ex.Message;
            }
            return res;

        }

        public ResponseModel Update(UsersModel userDto, string password = null)
        {
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var role = GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
            {
                var user = _context.TblUsers.Where(o => o.Id == userDto.Id && o.DeletedOn == null).FirstOrDefault();


                try
                {
                    if (user == null)
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = "User not found";
                        return res;
                    }
                    if (userDto.UserName != user.UserName)
                    {
                        // username has changed so check if the new username is already taken
                        if (_context.TblUsers.Any(x => x.UserName == userDto.UserName))
                        {
                            res.Data = "";
                            res.Status = "500";
                            res.Message = ("Username " + userDto.UserName + " is already taken");
                        }

                    }

                    // update user properties
                    user.FamilyName = userDto.FamilyName;
                    user.GivenName = userDto.GivenName;
                    user.UserName = userDto.UserName;
                    user.TypeOfAccount = userDto.TypeOfAccount;
                    user.PasswordHash = userDto.PasswordHash;
                    user.PasswordSalt = userDto.PasswordSalt;
                    user.Email = userDto.Email;
                    user.Cid = userDto.Cid;
                    user.RoleId = userDto.RoleID;
                    user.ContactByEmail = userDto.ContactByEmail;
                    user.EncryptionActive = userDto.EncryptionActive;

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
                    res.Data = userDto;

                }
                catch (Exception ex)
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = "Username " + userDto.UserName + " is already taken";

                }
                if (role == Constant.Users.User)
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

                        if (userDto.UserName != user2.UserName)
                        {
                            // username has changed so check if the new username is already taken
                            if (_context.TblUsers.Any(x => x.UserName == userDto.UserName))
                            {
                                res.Data = "";
                                res.Status = "500";
                                res.Message = ("Username " + userDto.UserName + " is already taken");
                            }

                        }

                        // update user properties

                        user2.FamilyName = userDto.FamilyName;
                        user2.GivenName = userDto.GivenName;
                        user2.UserName = userDto.UserName;
                        user2.TypeOfAccount = userDto.TypeOfAccount;
                        user2.PasswordHash = userDto.PasswordHash;
                        user2.PasswordSalt = userDto.PasswordSalt;
                        user2.Email = userDto.Email;
                        user2.Cid = userDto.Cid;
                        user2.RoleId = userDto.RoleID;
                        user2.ContactByEmail = userDto.ContactByEmail;
                        user2.EncryptionActive = userDto.EncryptionActive;

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
                        res.Data = userDto;

                    }
                    catch (Exception ex)
                    {
                        res.Data = "";
                        res.Status = "500";
                        res.Message = ("Username " + userDto.UserName + " is already taken");

                    }
                    return res;
                }
            }
            return res;
        }

        public ResponseModel UpdateNoPass(UsersModel userParam)
        {
            var context = _httpContextAccessor.HttpContext;
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            var role = GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
                    user.RoleId = userParam.RoleID;
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
            if (role == Constant.Users.User)
            {
                var user = _context.TblUsers.Where(o => o.Id == int.Parse(context.User.Identity.Name) && o.DeletedOn == null).FirstOrDefault();


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
                    user.RoleId = userParam.RoleID;
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
            var role = GetRole(int.Parse(context.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
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
            var role = GetRole(int.Parse(cont.User.Identity.Name));
            if (role == Constant.Users.Admin || role == Constant.Users.Superadmin)
            {

                try
                {
                    foreach (var deleteId in deleteIds)
                    {
                        var user = _context.TblUsers.FirstOrDefault(o => o.Id == deleteId);
                        if (user != null)
                        {
                            user.DeletedOn = DateTime.UtcNow;
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

        public int GetRole(int pUserID)
        {
            var roleid = _context.TblUsers.Where(o => o.Id == pUserID).Select(o => o.RoleId).FirstOrDefault();
            return roleid;
        }

        public string GetCID(string userID)
        {
            var strcid = int.Parse(userID);
            var cid = _context.TblUsers.Where(o => o.Id == strcid).Select(o => o.Cid).FirstOrDefault();
            return cid;
        }
        public TblUsers GetUser(string userid)
        {
            var userID = int.Parse(userid);
            var user = _context.TblUsers.Where(o => o.Id == userID).FirstOrDefault();
            return user;
        }
    }
}


