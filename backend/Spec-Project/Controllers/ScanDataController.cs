using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Scanx.Common;
using Scanx.Web.Services;

namespace Scanx.Web.Controllers
{
    [Route("api/ScanData")]
    [ApiController]
    public class ScanDataController : ControllerBase
    {
        private IScanDataService _IScanDataService;
        public ScanDataController(IScanDataService scandataService)
        {
            _IScanDataService = scandataService;
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
            return Ok(_IScanDataService.GetScanData());
        }

        [Authorize]
        [DisableCors]
        [HttpGet("check-scandata")]
        public IActionResult CheckUserScanData(string scandataID)
        {
            return Ok(_IScanDataService.CheckUserScanData(scandataID));
        }

        [DisableCors]
        [HttpPost("import-scandata")]
        public IActionResult ImportScanData([FromBody]ImportDataModel tblscandata)
        {
            return Ok(_IScanDataService.ImportScanData(tblscandata));
        }

        [Authorize]
        [DisableCors]
        [HttpPost("add-scandata")]
        public IActionResult addScanData(ScanDataModel tblscandata)
        {
            return Ok(_IScanDataService.AddScanData(tblscandata));
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