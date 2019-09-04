using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spec_Project.Entities;
using Spec_Project.Models;
using Spec_Project.Services;
using Spec_Project.Helpers;

namespace Spec_Project.Controllers
{
    [Authorize]
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
            IOptions<Helpers.AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(string username , string password)
        {
            var user = _userService.Authenticate(username, password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            if(_appSettings.Secret == null)
            {
                _appSettings.Secret = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJmaXJzdG5hbWUiOiJ0dXllbiIsImxhc3RuYW1lIjoibmd1eWVuIiwidXNlcm5hbWUiOiJnZWFyMjE5IiwicGFzc3dvcmQiOiIxMjMifQ.a9jA6viURjMOzqeT58R39ORgmNovPp0OkbGp9VkaoVg";
            }
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FamilyName,
                LastName = user.GivenName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            //var user = _mapper.Map<TblUsers>(userDto);

            try
            {
                // save 
                
                return Ok(_userService.Create(userDto, userDto.Password));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [DisableCors]
        [Authorize(Roles = RoleConstant.admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [DisableCors]
        [HttpGet("get-user-by-id")]
        public IActionResult GetById(int id)
        {
            if (!User.IsInRole(RoleConstant.admin))
            {
                return Forbid();
            }
            var user = _userService.GetById(id);
            var userDto = new UserDto
            {
                Id = user.Id,
                Cid = user.Cid,
                ContactByEmail = user.ContactByEmail,
                Email = user.Email,
                EncryptionActive = user.EncryptionActive,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                TypeOfAccount = user.TypeOfAccount,
                UserName = user.UserName

            };
            return Ok(userDto);
        }

        [DisableCors]
        [HttpPut("update")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            //var user = _mapper.Map<TblUsers>(userDto);
            byte[] passwordHa, passwordSa;
            UserService.CreatePasswordHash(userDto.Password, out passwordHa, out passwordSa);
            var user = new TblUsers
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
                PasswordSalt = passwordSa
            };
            user.Id = id;

            try
            {
                if (!User.IsInRole(RoleConstant.admin))
                {
                    return Forbid();
                }
                else
                {
                    _userService.Update(user, userDto.Password);
                }
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }

        [DisableCors]
        [Authorize(Roles = RoleConstant.admin)]
        [HttpDelete("delete-user")]
        public IActionResult Delete(int id)
        {

            var rs = _userService.Delete(id);
            return Ok(rs);
        }
    }
}