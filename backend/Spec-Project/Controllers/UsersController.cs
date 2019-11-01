using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scanx.Web.Models;
using Scanx.Web.Services;
using Microsoft.AspNetCore.Cors;
using System.Linq;

namespace Scanx.Web.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly Helpers.AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<Helpers.AppSettings> appSettings
            )
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [DisableCors]
        [HttpPost("authenticate")]
        public ResponseModel Authenticate(string username, string password)
        {
            try
            {
                var user = _userService.Authenticate(username, password, _appSettings);

                if (user == null)
                    return
                        /*BadRequest(new { message = "Username or password is incorrect" });*/
                        new ResponseModel()
                        {
                            Status = "400",
                            Message = "Username or password is incorrect",
                        };
                var companyName = string.Empty;

                // return basic user info (without password) and token to store client side
                return new ResponseModel()
                {
                    Status = "200",
                    Data = new
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FamilyName = user.FamilyName,
                        GivenName = user.GivenName,
                        CompanyName = user.CompanyName,
                        Mail = user.Mail,
                        TypeOfAccount = user.TypeOfAccount,
                        RoleID = user.RoleID,
                        Authorization = user.Authorization,
                        Token = user.Token
                    },
                    Message = "Login successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = "500",
                    Message = "Error: " + ex.Message,
                };
            }
        }

        [AllowAnonymous]
        [DisableCors]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto, string UserIDLogin)
        {
            // map dto to entity
            //var user = _mapper.Map<TblUsers>(userDto);

            try
            {
                // save 
                return Ok(_userService.Create(userDto, userDto.Password, UserIDLogin));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [DisableCors]
        [HttpPost("adduser")]
        public ResponseModel AddUser([FromBody]UserDto userDto, string password, string UserIDLogin)
        {
            // map dto to entity
            //var user = _mapper.Map<TblUsers>(userDto);
            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });
            try
            {
                var x = _userService.AddUser(userDto, userDto.Password, UserIDLogin);
                // save 
                //return Ok(_userService.AddUser(userDto, userDto.Password, UserIDLogin));
                res.Data = x;
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                //return BadRequest(new { message = ex.Message });
                res.Message = "false";

            }
            return res;
        }

        [Authorize]
        [DisableCors]

        [HttpGet("get-all-user")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }


        [Authorize]
        [DisableCors]
        [HttpGet("get-user")]
        public IActionResult GetUser()
        {
            return Ok(_userService.GetUser());
        }

        [DisableCors]

        [HttpGet("get-user-by-id")]
        public ResponseModel GetById(int id)
        {
            var res = _userService.GetById(id);

            return res;
        }



        [Authorize]
        [DisableCors]
        [HttpPut("update")]
        public ResponseModel Update([FromBody]UsersModel userDto)
        {
            // map dto to entity and set id
            //var user = _mapper.Map<TblUsers>(userDto);

            ResponseModel res = (new ResponseModel
            {
                Data = "",
                Status = "200",
                Message = ""
            });

            var password = userDto.Password;
            if (password != null && password != "")
            {
                byte[] passwordHa, passwordSa;
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSa = hmac.Key;
                    passwordHa = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
                var user = new UsersModel
                {
                    Id = userDto.Id,
                    Cid = userDto.Cid,
                    ContactByEmail = userDto.ContactByEmail,
                    Email = userDto.Email,
                    EncryptionActive = userDto.EncryptionActive,
                    FamilyName = userDto.FamilyName,
                    GivenName = userDto.GivenName,
                    TypeOfAccount = userDto.TypeOfAccount,
                    UserName = userDto.UserName,
                    PasswordHash = passwordHa,
                    PasswordSalt = passwordSa,
                    RoleID = userDto.RoleID
                };
                //user.Id = id;

                try
                {
                    _userService.Update(user, userDto.Password);
                    res.Data = user;
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    //return BadRequest(new { message = ex.Message });
                    res.Message = "false";

                }
            }
            else
            {
                var user = new UsersModel
                {
                    Id = userDto.Id,
                    Cid = userDto.Cid,
                    ContactByEmail = userDto.ContactByEmail,
                    Email = userDto.Email,
                    EncryptionActive = userDto.EncryptionActive,
                    FamilyName = userDto.FamilyName,
                    GivenName = userDto.GivenName,
                    TypeOfAccount = userDto.TypeOfAccount,
                    UserName = userDto.UserName,
                    RoleID = userDto.RoleID
                };

                //user.Id = id;

                try
                {
                    _userService.UpdateNoPass(user);
                    res.Data = user;
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    //return BadRequest(new { message = ex.Message });
                    res.Message = "false";

                }
            }
            return res;
        }

        [Authorize]
        [DisableCors]
        [HttpDelete("delete-user")]
        public IActionResult Delete(int id)
        {

            var rs = _userService.Delete(id);
            return Ok(rs);
        }

        [Authorize]
        [DisableCors]
        [HttpPost("delete-arr-user")]
        public IActionResult DeleteArrUser(List<int> deleteIds)
        {
            return Ok(_userService.DeleteArrUser(deleteIds));
        }
    }
}