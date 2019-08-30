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
        IEnumerable<TblUsers> GetAll();
        TblUsers GetById(int id);
        ResponseModel Create(UserDto user, string password);
        ResponseModel Update(TblUsers user, string password = null);
        ResponseModel Delete(int id);
    }

    public class UserService : IUserService
    {
        DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public TblUsers Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.TblUsers.FirstOrDefault(x => x.UserName == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!IsCorrectPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<TblUsers> GetAll()
        {
            return _context.TblUsers;
        }

        public TblUsers GetById(int id)
        {
            return _context.TblUsers.Find(id);
        }

        public ResponseModel Create(UserDto user, string password)
        {
            var tbluser = new TblUsers
            {

                UserName = user.UserName,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                TypeOfAccount = user.TypeOfAccount,
                Email = user.Email,
                ContactByEmail = user.ContactByEmail,
                EncryptionActive = user.EncryptionActive

            };
            ResponseModel res = (new ResponseModel {
                Data = "",
                Status = "200",
                Message = ""
            });
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

                if (_context.TblUsers.Any(x => x.UserName == user.UserName))
                {
                    res.Data = "";
                    res.Status = "500";
                    res.Message = "Username \"" + user.UserName + "\" is already taken";
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
            return res;
        }
        


        public ResponseModel Update(TblUsers userParam, string password = null)
        {
            var user = _context.TblUsers.Find(userParam.Id);

            ResponseModel res = (new ResponseModel {
                Data = "",
                Status = "200",
                Message = ""

            });
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
                user.ContactByEmail = userParam.ContactByEmail;
                user.EncryptionActive = userParam.EncryptionActive;

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

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

        public ResponseModel Delete(int id)
        {
            var user = _context.TblUsers.Find(id);
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "500",
                Message = "ID not found"

            });
           
            if (user != null)
            {
                _context.TblUsers.Remove(user);
                _context.SaveChanges();
                res.Status = "200";
                res.Message = "";
                return res;
            }
            return res;
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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
    }
}
