using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Spec_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Spec_Project.Controllers
{

    [Route("api/Start")]
    [ApiController]
    public class StartController : ControllerBase
    {
        private IStartService _IStartService;
        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public StartController(
        IStartService startService)
        {
            _IStartService = startService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }
        [HttpGet("get-user-by-id")]
        public IActionResult getUser(string userID)
        {
            return Ok(_IStartService.Login(userID));
        }

        [HttpGet("get-data-scan-by-userid")]
        public IActionResult getDataScanInput(string userID)
        {
            return Ok(_IStartService.getDataScanInput(userID));
        }
        [HttpPost("add-user")]
        public IActionResult AddUser()
        {
            return Ok(_IStartService.AddUser());
        }
        [HttpPost("edit-user")]
        public IActionResult EditUser()
        {
            return Ok(_IStartService.);
        }
    }
}