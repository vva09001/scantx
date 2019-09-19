﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Spec_Project.Entities;
using Spec_Project.Models;
using Spec_Project.Services;

namespace Spec_Project.Controllers
{
    [Authorize]
    [Route("api/ScanData")]
    [ApiController]
    public class ScanDataController : ControllerBase
    {
        IHttpContextAccessor _httpContextAccessor;
        private IScanDataService _IScanDataService;
        DataContext context = new DataContext();
        IScanDataService _object;
        private IScanDataService @object;

        //private IMapper _mapper;
        //private readonly AppSettings _appSettings;
        public ScanDataController(IScanDataService scandataService)
        {
            _IScanDataService = scandataService;
            //_mapper = mapper;
            //_appSettings = appSettings.Value;
        }

       


        //[Authorize]
        [DisableCors]
        [HttpGet("convert-scandata-to-csv")]
        public IActionResult ConvertTblScanDataToCSV(int uid)
        {
            return Ok(_IScanDataService.ConvertTblScanDataToCSV(uid));
        }

        [Authorize]
        [DisableCors]
        [HttpGet("get-scandata")]
        public IActionResult getScanData()
        {
            return Ok(_IScanDataService.getScanData());
        }

        [Authorize]
        [DisableCors]
        [HttpGet("check-scandata")]
        public IActionResult CheckUserScanData(string scandataID)
        {
            return Ok(_IScanDataService.CheckUserScanData(scandataID));
        }

        [Authorize]
        [DisableCors]
        [HttpPost("add-scandata")]
        public IActionResult addScanData(ScanDataModel tblscandata)
        {
            return Ok(_IScanDataService.addScanData(tblscandata));
        }

        [Authorize]
        [DisableCors]
        [HttpPost("delete-scandata")]
        public IActionResult DeleteScanData(string scanid)
        {
            return Ok(_IScanDataService.DeleteScanData(scanid));
        }

        [DisableCors]
        [HttpPost("delete-arr-scandata")]
        public IActionResult DeleteArrScanData(List<string> deleteIds)
        {
            return Ok(_IScanDataService.DeleteArrScanData(deleteIds));
        }

        [Authorize]
        [DisableCors]
        [HttpPut("edit-scandata")]
        public IActionResult EditScandata(ScanDataModel tblscandata)
        {
            return Ok(_IScanDataService.EditScandata(tblscandata));
        }

        [Authorize]
        [DisableCors]
        [HttpGet("getqr")]
        public IActionResult GetQr()
        {
            return Ok(_IScanDataService.CreateQR());
        }

        
    }
}