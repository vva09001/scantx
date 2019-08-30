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
using Spec_Project.Entities;
using Spec_Project.Services;

namespace Spec_Project.Controllers
{

    [Route("api/ScanData")]
    [ApiController]
    public class ScanDataController : ControllerBase
    {
        private IScanDataService _IScanDataService;
        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public ScanDataController(
        IScanDataService scandataService)
        {
            _IScanDataService = scandataService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }

        [DisableCors]
        [HttpGet("get-scandata")]
        public IActionResult getScanData()
        {
            return Ok(_IScanDataService.getScanData());
        }

        [DisableCors]
        [HttpPost("add-scandata")]
        public IActionResult addScanData(TblScanData tblscandata)
        {
            return Ok(_IScanDataService.addScanData(tblscandata));
        }

        [DisableCors]
        [HttpPost("delete-scandata")]
        public IActionResult DeleteScanData(string scanid)
        {
            return Ok(_IScanDataService.DeleteScanData(scanid));
        }

        [DisableCors]
        [HttpPut("edit-scandata")]
        public IActionResult EditScandata(TblScanData tblscandata)
        {
            return Ok(_IScanDataService.EditScandata(tblscandata));
        }
    }
}